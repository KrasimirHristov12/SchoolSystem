namespace SchoolSystem.Services.Data.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Quizzes;

    public interface IQuizzesService
    {
        Task AddAsync(QuizzesInputModel model, int teacherId);

        IEnumerable<DisplayQuizzesViewModel> GetMine(int studentId, string date);

        GetQuizResult GetQuiz(Guid id, int studentId);

        //Task<IEnumerable<AnswersViewModel>> GetAnswersAsync(Guid id);

        Task RecordAsDoneAsync(int studentId, Guid quizId, int points, IEnumerable<AnswersViewModel> answers);

        ReviewQuizViewModel GetReviewQuiz(Guid quizId, int studentId);

        Task<int?> MarkAsCorrectAsync(Guid quizId, int studentId);
    }
}
