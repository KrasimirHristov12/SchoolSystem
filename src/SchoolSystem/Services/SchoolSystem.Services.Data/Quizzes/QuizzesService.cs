namespace SchoolSystem.Services.Data.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Data.GradingScale;
    using SchoolSystem.Services.Data.Notifications;
    using SchoolSystem.Services.Data.Questions;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.ViewModels.Questions;
    using SchoolSystem.Web.ViewModels.Quizzes;

    public class QuizzesService : IQuizzesService
    {
        private readonly IDeletableEntityRepository<SchoolClass> classesRepo;
        private readonly IDeletableEntityRepository<Student> studentsRepo;
        private readonly IDeletableEntityRepository<Quiz> quizzesRepo;
        private readonly IDeletableEntityRepository<StudentsQuizzes> studentsQuizzesRepo;
        private readonly IDeletableEntityRepository<Question> questionsRepo;
        private readonly IDeletableEntityRepository<StudentQuizzesQuestionAnswer> studentQuizzesQuestionAnswersRepo;
        private readonly IQuestionsService questionsService;
        private readonly IGradingScaleService gradingScaleService;
        private readonly IStudentService studentService;
        private readonly ITeacherService teacherService;
        private readonly INotificationsService notificationsService;

        public QuizzesService(IDeletableEntityRepository<SchoolClass> classesRepo, IDeletableEntityRepository<Student> studentsRepo, IDeletableEntityRepository<Quiz> quizzesRepo, IDeletableEntityRepository<StudentsQuizzes> studentsQuizzesRepo, IDeletableEntityRepository<Question> questionsRepo, IDeletableEntityRepository<StudentQuizzesQuestionAnswer> studentQuizzesQuestionAnswersRepo, IQuestionsService questionsService, IGradingScaleService gradingScaleService, IStudentService studentService, ITeacherService teacherService, INotificationsService notificationsService)
        {
            this.classesRepo = classesRepo;
            this.studentsRepo = studentsRepo;
            this.quizzesRepo = quizzesRepo;
            this.studentsQuizzesRepo = studentsQuizzesRepo;
            this.questionsRepo = questionsRepo;
            this.studentQuizzesQuestionAnswersRepo = studentQuizzesQuestionAnswersRepo;
            this.questionsService = questionsService;
            this.gradingScaleService = gradingScaleService;
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.notificationsService = notificationsService;
        }

        public async Task AddAsync(QuizzesInputModel model, int teacherId)
        {
            var classes = this.classesRepo.All().Where(c => model.ClassesIds.Contains(c.Id)).ToList();

            var quiz = new Quiz()
            {
                SubjectId = (int)model.SubjectId,
                Classes = classes,
                Name = model.Name,
                Duration = TimeSpan.FromMinutes((double)model.Duration),
                DateTaken = TimeZoneInfo.ConvertTimeToUtc((DateTime)model.DateTaken, TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time")),
                QuizType = (QuizType)model.QuizType,
                TeacherId = teacherId,
            };

            var questions = new List<Question>();

            foreach (var q in model.Questions)
            {
                var question = q.Map<Question>();

                questions.Add(question);
            }

            quiz.Questions = questions;
            await this.quizzesRepo.AddAsync(quiz);
            await this.quizzesRepo.SaveChangesAsync();

            await this.gradingScaleService.AddAsync(quiz.Id, model.ScaleRangeForPoor, model.ScaleRangeForFair, model.ScaleRangeForGood, model.ScaleRangeForVeryGood, model.ScaleRangeForExcellent);

            await this.SendNotificationAsync(model.ClassesIds, teacherId, model.Name);
        }

        public IEnumerable<T> GetMine<T>(int studentId, string date)
        {
            var studentClassId = this.classesRepo.All().Where(c => c.Students.Any(s => s.Id == studentId)).Select(c => c.Id).FirstOrDefault();
            if (studentClassId == 0)
            {
                return new List<T>();
            }

            var foundQuizzes = this.quizzesRepo.AllAsNoTracking().Where(q => q.Classes.Any(c => c.Id == studentClassId) && DateTime.Compare(q.DateTaken.Date, DateTime.Parse(date)) == 0).To<T>(new
            {
                studentId = studentId,
            }).ToList();

            return foundQuizzes;
        }

        public GetQuizResult<T> GetQuiz<T>(Guid id, int studentId)
        {
            var quiz = this.quizzesRepo.AllAsNoTracking().Where(q => q.Id == id).To<T>(new
            {
                studentId = studentId,
            }).FirstOrDefault();

            if (quiz == null)
            {
                return new GetQuizResult<T>
                {
                    IsSuccessful = false,
                    Message = GlobalConstants.ErrorMessage.QuizDoesNotExist,
                };
            }

            if (this.studentsQuizzesRepo.AllAsNoTracking().Any(sq => sq.QuizId == id && sq.StudentId == studentId))
            {
                return new GetQuizResult<T>
                {
                    IsSuccessful = false,
                    Message = GlobalConstants.ErrorMessage.QuizAlreadyTaken,
                };
            }

            // if (DateTime.Compare(DateTime.UtcNow, quiz.DateTaken.AddMinutes(quiz.Duration)) >= 0)
            // {
            //     return new GetQuizResult
            //     {
            //         IsSuccessful = false,
            //         Message = GlobalConstants.ErrorMessage.QuizDue,
            //     };
            // }

            // if (DateTime.Compare(DateTime.UtcNow, quiz.DateTaken) < 0)
            // {
            //     return new GetQuizResult
            //     {
            //         IsSuccessful = false,
            //         Message = quiz.DateTaken.Subtract(DateTime.UtcNow).TotalHours >= 1 ? string.Format(GlobalConstants.ErrorMessage.QuizNotStartedYetHours, Math.Round(quiz.DateTaken.Subtract(DateTime.UtcNow).TotalHours))
            //         : string.Format(GlobalConstants.ErrorMessage.QuizNotStartedYetMinutes, Math.Round(quiz.DateTaken.Subtract(DateTime.UtcNow).TotalMinutes)),
            //     };
            // }

            return new GetQuizResult<T>
            {
                IsSuccessful = true,
                Message = string.Empty,
                Model = quiz,
            };
        }

        public IEnumerable<ReviewQuizViewModel> GetReviewQuizModel(Guid quizId, int studentId)
        {
            var reviewQuizViewModelCollection = new List<ReviewQuizViewModel>();
            var questionsIds = this.questionsService.GetIdsByQuizId(quizId).ToList();
            var questions = this.questionsRepo.AllAsNoTracking().Where(q => q.QuizId == quizId).ToList();

            var questionAnswersFromStudent = this.studentQuizzesQuestionAnswersRepo.AllAsNoTracking().Where(q => questionsIds.Contains(q.QuestionId) && q.StudentId == studentId).ToList();

            // THE STUDENT HAS NOT TAKEN THE EXAM - DISPLAY APPROPRIATE ERROR PAGE SOON.
            if (questionAnswersFromStudent.Count == 0)
            {
                return reviewQuizViewModelCollection;
            }

            for (int i = 0; i < questions.Count; i++)
            {
                string actualAnswers = string.Empty;

                var correctAnswers = new List<bool>
                {
                    questions[i].IsFirstAnswerCorrect, questions[i].IsSecondAnswerCorrect, questions[i].IsThirdAnswerCorrect, questions[i].IsFourthAnswerCorrect,
                };
                var studentAnswers = new List<bool>
                {
                    questionAnswersFromStudent[i].IsFirstAnswerChecked, questionAnswersFromStudent[i].IsSecondAnswerChecked, questionAnswersFromStudent[i].IsThirdAnswerChecked, questionAnswersFromStudent[i].IsFourthAnswerChecked,
                };
                bool isCorrect = correctAnswers.SequenceEqual(studentAnswers);

                if (!isCorrect)
                {
                    var actualAnswersList = new List<char>();
                    if (questions[i].IsFirstAnswerCorrect)
                    {
                        actualAnswersList.Add('а');
                    }

                    if (questions[i].IsSecondAnswerCorrect)
                    {
                        actualAnswersList.Add('б');
                    }

                    if (questions[i].IsThirdAnswerCorrect)
                    {
                        actualAnswersList.Add('в');
                    }

                    if (questions[i].IsFourthAnswerCorrect)
                    {
                        actualAnswersList.Add('г');
                    }

                    actualAnswers = string.Join(",", actualAnswersList);
                }

                var reviewQuizViewModel = new ReviewQuizViewModel
                {
                    Title = questions[i].Title,
                    Type = questions[i].Type,
                    FirstAnswerContent = questions[i].FirstAnswerContent,
                    SecondAnswerContent = questions[i].SecondAnswerContent,
                    ThirdAnswerContent = questions[i].ThirdAnswerContent,
                    FourthAnswerContent = questions[i].FourthAnswerContent,
                    IsFirstAnswerChecked = questionAnswersFromStudent[i].IsFirstAnswerChecked,
                    IsSecondAnswerChecked = questionAnswersFromStudent[i].IsSecondAnswerChecked,
                    IsThirdAnswerChecked = questionAnswersFromStudent[i].IsThirdAnswerChecked,
                    IsFourthAnswerChecked = questionAnswersFromStudent[i].IsFourthAnswerChecked,
                    IsAnswerCorrect = isCorrect,
                    ActualAnswers = actualAnswers,
                    EarnedPoints = isCorrect ? questions[i].Points : 0,
                };
                reviewQuizViewModelCollection.Add(reviewQuizViewModel);
            }

            return reviewQuizViewModelCollection;
        }

        public async Task<int?> MarkAsCorrectAsync(Guid quizId, int studentId)
        {
            var studentQuiz = this.studentsQuizzesRepo.All().FirstOrDefault(sq => sq.QuizId == quizId && sq.StudentId == studentId);
            if (studentQuiz == null)
            {
                return null;
            }

            studentQuiz.Points += 1;
            await this.studentsQuizzesRepo.SaveChangesAsync();
            return studentQuiz.Points;
        }

        public async Task RecordAsDoneAsync(int studentId, Guid quizId, int points)
        {
            var studentQuiz = new StudentsQuizzes
            {
                StudentId = studentId,
                QuizId = quizId,
                IsTaken = true,
                Points = points,
            };
            await this.studentsQuizzesRepo.AddAsync(studentQuiz);

            await this.studentsQuizzesRepo.SaveChangesAsync();
        }

        public async Task<int> RecordAnswersAsync(Guid quizId, int studentId, List<TakeQuestionsViewModel> questions)
        {
            foreach (var question in questions)
            {
                if (!this.questionsRepo.AllAsNoTracking().Any(q => q.Id == question.Id))
                {
                    return -1;
                }

                var dataObj = new StudentQuizzesQuestionAnswer
                {
                    StudentId = studentId,
                    QuestionId = question.Id,
                    IsFirstAnswerChecked = question.IsFirstAnswerChecked,
                    IsSecondAnswerChecked = question.IsSecondAnswerChecked,
                    IsThirdAnswerChecked = question.IsThirdAnswerChecked,
                    IsFourthAnswerChecked = question.IsFourthAnswerChecked,
                };
                await this.studentQuizzesQuestionAnswersRepo.AddAsync(dataObj);
            }

            var quizQuestions = this.questionsRepo.AllAsNoTracking().Where(q => q.QuizId == quizId).ToList();

            var studentPoints = this.GetPoints(quizQuestions, questions);

            await this.RecordAsDoneAsync(studentId, quizId, studentPoints);

            return studentPoints;
        }

        private int GetPoints(List<Question> quizQuestions, List<TakeQuestionsViewModel> studentQuestions)
        {
            int points = 0;
            foreach (var question in quizQuestions)
            {
                var studentQuestion = studentQuestions.FirstOrDefault(q => q.Id == question.Id);
                if (studentQuestion != null)
                {
                    var correctAnswers = new List<bool>
                    {
                        question.IsFirstAnswerCorrect, question.IsSecondAnswerCorrect, question.IsThirdAnswerCorrect, question.IsFourthAnswerCorrect,
                    };

                    var studentAnswers = new List<bool>
                    {
                        studentQuestion.IsFirstAnswerChecked, studentQuestion.IsSecondAnswerChecked, studentQuestion.IsThirdAnswerChecked, studentQuestion.IsFourthAnswerChecked,
                    };

                    bool areEqual = correctAnswers.SequenceEqual(studentAnswers);

                    if (areEqual)
                    {
                        points += question.Points;
                    }
                }
            }

            return points;
        }

        private async Task SendNotificationAsync(IEnumerable<int?> classesIds, int teacherId, string quizName)
        {
            var studentsUserIds = new List<string>();
            var teacherFullName = this.teacherService.GetTeacherFullName(teacherId);
            foreach (var classId in classesIds)
            {
                var studentsInThisClass = this.studentsRepo.AllAsNoTracking().Where(st => st.ClassId == classId).Select(st => st.Id).ToList();
                foreach (var studentId in studentsInThisClass)
                {
                    var studentUserId = this.studentService.GetUserId(studentId);
                    studentsUserIds.Add(studentUserId);
                }
            }

            string message = string.Format(GlobalConstants.Notification.TestAdded, teacherFullName, quizName);
            await this.notificationsService.AddAsync(NotificationType.AddedTest, studentsUserIds, message);
        }
    }
}
