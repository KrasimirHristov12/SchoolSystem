namespace SchoolSystem.Web.ViewModels.Quizzes
{
    public class GetQuizResult<T>
    {
        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public T Model { get; set; }
    }
}
