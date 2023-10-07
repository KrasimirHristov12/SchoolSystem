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

        IEnumerable<T> GetMine<T>(int studentId, string date);

        GetQuizResult<T> GetQuiz<T>(Guid id, int studentId);

        //Task<IEnumerable<AnswersViewModel>> GetAnswersAsync(Guid id);

        Task<int> RecordAnswersAsync(Guid quizId, int studentId, List<TakeQuestionsViewModel> questions);

        Task RecordAsDoneAsync(int studentId, Guid quizId, int points);

        IEnumerable<ReviewQuizViewModel> GetReviewQuizModel(Guid quizId, int studentId);

        Task<int?> MarkAsCorrectAsync(Guid quizId, int studentId);
    }
}
