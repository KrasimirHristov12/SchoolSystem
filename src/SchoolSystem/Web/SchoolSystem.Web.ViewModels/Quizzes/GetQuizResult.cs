namespace SchoolSystem.Web.ViewModels.Quizzes
{
    public class GetQuizResult
    {
        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public TakeQuizViewModel Model { get; set; }
    }
}
