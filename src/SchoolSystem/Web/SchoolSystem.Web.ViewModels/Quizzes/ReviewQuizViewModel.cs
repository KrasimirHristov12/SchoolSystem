namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using SchoolSystem.Data.Models.Enums;

    public class ReviewQuizViewModel
    {
        public string Title { get; set; }

        public QuestionType Type { get; set; }

        public string FirstAnswerContent { get; set; }

        public bool IsFirstAnswerChecked { get; set; }

        public string SecondAnswerContent { get; set; }

        public bool IsSecondAnswerChecked { get; set; }

        public string ThirdAnswerContent { get; set; }

        public bool IsThirdAnswerChecked { get; set; }

        public string FourthAnswerContent { get; set; }

        public bool IsFourthAnswerChecked { get; set; }

        public bool IsAnswerCorrect { get; set; }

        public string ActualAnswers { get; set; }

        public int EarnedPoints { get; set; }
    }
}
