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
                Content = model.Content,
                Answers = model.Answers,
                Classes = classes,
                Name = model.Name,
                Duration = TimeSpan.FromMinutes((double)model.Duration),
                DateTaken = TimeZoneInfo.ConvertTimeToUtc((DateTime)model.DateTaken, TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time")),
                QuizType = (QuizType)model.QuizType,
                TeacherId = teacherId,
            };

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
                Duration = q.Duration.Minutes,
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
                Content = q.Content,
                StudentId = studentId,
                Duration = q.Duration.Minutes,
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

            if (DateTime.Compare(DateTime.UtcNow, quiz.DateTaken.AddMinutes(quiz.Duration)) >= 0)
            {
                return new GetQuizResult
                {
                    IsSuccessful = false,
                    Message = GlobalConstants.ErrorMessage.QuizDue,
                };
            }

            if (DateTime.Compare(DateTime.UtcNow, quiz.DateTaken) < 0)
            {
                return new GetQuizResult
                {
                    IsSuccessful = false,
                    Message = quiz.DateTaken.Subtract(DateTime.UtcNow).TotalHours >= 1 ? string.Format(GlobalConstants.ErrorMessage.QuizNotStartedYetHours, Math.Round(quiz.DateTaken.Subtract(DateTime.UtcNow).TotalHours))
                    : string.Format(GlobalConstants.ErrorMessage.QuizNotStartedYetMinutes, Math.Round(quiz.DateTaken.Subtract(DateTime.UtcNow).TotalMinutes)),
                };
            }

            return new GetQuizResult
            {
                IsSuccessful = true,
                Message = string.Empty,
                Model = quiz,
            };
        }

        public async Task<IEnumerable<AnswersViewModel>> GetAnswersAsync(Guid id)
        {
            var quiz = await this.db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<IEnumerable<AnswersViewModel>>(quiz.Answers, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
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
    }
}
