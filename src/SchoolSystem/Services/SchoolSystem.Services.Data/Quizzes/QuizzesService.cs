namespace SchoolSystem.Services.Data.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.ViewModels.Answers;
    using SchoolSystem.Web.ViewModels.Questions;
    using SchoolSystem.Web.ViewModels.Quizzes;

    public class QuizzesService : IQuizzesService
    {
        private readonly ApplicationDbContext db;

        public QuizzesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(QuizzesInputModel model, int teacherId)
        {
            var classes = this.db.Classes.Where(c => model.ClassesId.Contains(c.Id)).ToList();
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
                var question = new Question
                {
                    Title = q.Title,
                    Type = (QuestionType)q.QuestionType,
                    FirstAnswerContent = q.Answers.FirstAnswerContent,
                    IsFirstAnswerCorrect = q.Answers.IsFirstAnswerCorrect,
                    SecondAnswerContent = q.Answers.SecondAnswerContent,
                    IsSecondAnswerCorrect = q.Answers.IsSecondAnswerCorrect,
                    ThirdAnswerContent = q.Answers.ThirdAnswerContent,
                    IsThirdAnswerCorrect = q.Answers.IsThirdAnswerCorrect,
                    FourthAnswerContent = q.Answers.FourthAnswerContent,
                    IsFourthAnswerCorrect = q.Answers.IsFourthAnswerCorrect,
                };

                questions.Add(question);
            }

            quiz.Questions = questions;
            this.db.Quizzes.Add(quiz);
            await this.db.SaveChangesAsync();
        }

        public IEnumerable<DisplayQuizzesViewModel> GetMine(int studentId, string date)
        {
            var foundClass = this.db.Classes.FirstOrDefault(c => c.Students.Any(s => s.Id == studentId));
            if (foundClass == null)
            {
                return new List<DisplayQuizzesViewModel>();
            }

            var classId = foundClass.Id;
            var foundQuizzes = this.db.Quizzes.Where(q => q.Classes.Any(c => c.Id == classId) && DateTime.Compare(q.DateTaken.Date, DateTime.Parse(date)) == 0).Select(q => new DisplayQuizzesViewModel
            {
                Id = q.Id,
                Name = q.Name,
                DateTaken = q.DateTaken,
                Duration = (int)q.Duration.TotalMinutes,
                SubjectName = q.Subject.Name,
                TeacherName = $"{q.Teacher.FirstName} {q.Teacher.LastName}",
                IsTaken = this.db.StudentsQuizzes.Where(sq => sq.StudentId == studentId && sq.QuizId == q.Id).Select(sq => sq.IsTaken).FirstOrDefault(),
                Points = this.db.StudentsQuizzes.Where(sq => sq.StudentId == studentId && sq.QuizId == q.Id).Select(sq => sq.Points).FirstOrDefault(),
            }).ToList();

            return foundQuizzes;
        }

        public GetQuizResult GetQuiz(Guid id, int studentId)
        {
            var quiz = this.db.Quizzes.Where(q => q.Id == id).Select(q => new TakeQuizViewModel
            {
                Id = id,
                Questions = this.db.Questions.Where(qu => qu.QuizId == id).Select(qu => new TakeQuestionsViewModel
                {
                    Id = qu.Id,
                    Title = qu.Title,
                    Type = qu.Type,
                    FirstAnswerContent = qu.FirstAnswerContent,
                    SecondAnswerContent = qu.SecondAnswerContent,
                    ThirdAnswerContent = qu.ThirdAnswerContent,
                    FourthAnswerContent = qu.FourthAnswerContent,
                    Points = qu.Points,

                }).ToList(),
                Duration = (int)q.Duration.TotalMinutes,
                DateTaken = q.DateTaken,

            }).FirstOrDefault();

            if (quiz == null)
            {
                return new GetQuizResult
                {
                    IsSuccessful = false,
                    Message = GlobalConstants.ErrorMessage.QuizDoesNotExist,
                };
            }

            if (this.db.StudentsQuizzes.Any(sq => sq.QuizId == id && sq.StudentId == studentId))
            {
                return new GetQuizResult
                {
                    IsSuccessful = false,
                    Message = GlobalConstants.ErrorMessage.QuizAlreadyTaken,
                };
            }

            //if (DateTime.Compare(DateTime.UtcNow, quiz.DateTaken.AddMinutes(quiz.Duration)) >= 0)
            //{
            //    return new GetQuizResult
            //    {
            //        IsSuccessful = false,
            //        Message = GlobalConstants.ErrorMessage.QuizDue,
            //    };
            //}

            //if (DateTime.Compare(DateTime.UtcNow, quiz.DateTaken) < 0)
            //{
            //    return new GetQuizResult
            //    {
            //        IsSuccessful = false,
            //        Message = quiz.DateTaken.Subtract(DateTime.UtcNow).TotalHours >= 1 ? string.Format(GlobalConstants.ErrorMessage.QuizNotStartedYetHours, Math.Round(quiz.DateTaken.Subtract(DateTime.UtcNow).TotalHours))
            //        : string.Format(GlobalConstants.ErrorMessage.QuizNotStartedYetMinutes, Math.Round(quiz.DateTaken.Subtract(DateTime.UtcNow).TotalMinutes)),
            //    };
            //}

            return new GetQuizResult
            {
                IsSuccessful = true,
                Message = string.Empty,
                Model = quiz,
            };
        }

        //public async Task<IEnumerable<AnswersViewModel>> GetAnswersAsync(Guid id)
        //{
        //    var quiz = await this.db.Quizzes.FindAsync(id);
        //    if (quiz == null)
        //    {
        //        return null;
        //    }

        //    return JsonSerializer.Deserialize<IEnumerable<AnswersViewModel>>(quiz.Answers, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true,
        //    });
        //}



        public ReviewQuizViewModel GetReviewQuiz(Guid quizId, int studentId)
        {
            //string quizContent = this.db.Quizzes.Where(q => q.Id == quizId).Select(q => q.Content).FirstOrDefault();
            //string correctAnswers = this.db.Quizzes.Where(q => q.Id == quizId).Select(q => q.Answers).FirstOrDefault();
            //var givenAnswers = this.db.StudentsQuizzes.Where(q => q.QuizId == quizId && q.StudentId == studentId).Select(q => q.Answers).FirstOrDefault();
            //if (quizContent == null || correctAnswers == null || givenAnswers == null)
            //{
            //    return null;
            //}

            //return new ReviewQuizViewModel
            //{
            //    Content = quizContent,
            //    CorrectAnswers = correctAnswers,
            //    GivenAnswers = givenAnswers,
            //};

            return null;
        }

        public async Task<int?> MarkAsCorrectAsync(Guid quizId, int studentId)
        {
            var studentQuiz = this.db.StudentsQuizzes.FirstOrDefault(sq => sq.QuizId == quizId && sq.StudentId == studentId);
            if (studentQuiz == null)
            {
                return null;
            }

            studentQuiz.Points += 1;
            await this.db.SaveChangesAsync();
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
            await this.db.StudentsQuizzes.AddAsync(studentQuiz);

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> RecordAnswersAsync(Guid quizId, int studentId, List<TakeQuestionsViewModel> questions)
        {
            foreach (var question in questions)
            {
                if (!this.db.Questions.Any(q => q.Id == question.Id))
                {
                    return false;
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
                this.db.StudentQuizzesQuestionAnswers.Add(dataObj);
            }

            var studentPoints = this.GetPoints(quizId, questions);

            await this.RecordAsDoneAsync(studentId, quizId, studentPoints);

            return true;
        }

        private int GetPoints(Guid quizId, List<TakeQuestionsViewModel> studentQuestions)
        {
            int points = 0;
            var quizQuestions = this.db.Questions.Where(q => q.QuizId == quizId).ToList();
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
                        studentQuestion.IsFirstAnswerChecked, question.IsSecondAnswerCorrect, question.IsThirdAnswerCorrect, question.IsFourthAnswerCorrect,
                    };

                    bool areEqual = correctAnswers.SequenceEqual(studentAnswers);

                    if (areEqual)
                    {
                        points += 1;
                    }
                }
            }

            return points;
        }
    }
}
