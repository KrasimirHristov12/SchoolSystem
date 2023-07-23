namespace SchoolSystem.Services.Data.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Questions;
    using SchoolSystem.Web.ViewModels.Quizzes;

    public interface IQuizzesService
    {
        Task AddAsync(QuizzesInputModel model, int teacherId);

        IEnumerable<DisplayQuizzesViewModel> GetMine(int studentId, string date);

        GetQuizResult GetQuiz(Guid id, int studentId);

        //Task<IEnumerable<AnswersViewModel>> GetAnswersAsync(Guid id);

        Task<bool> RecordAnswersAsync(Guid quizId, int studentId, List<TakeQuestionsViewModel> questions);

        Task RecordAsDoneAsync(int studentId, Guid quizId, int points);

        ReviewQuizViewModel GetReviewQuiz(Guid quizId, int studentId);

        Task<int?> MarkAsCorrectAsync(Guid quizId, int studentId);
    }
}
