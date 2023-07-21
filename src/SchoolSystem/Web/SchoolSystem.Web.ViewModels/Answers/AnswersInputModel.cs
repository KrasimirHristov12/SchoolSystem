namespace SchoolSystem.Web.ViewModels.Answers
{
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;

    public class AnswersInputModel
    {
        [RequiredWithErrorMessage]
        public string FirstAnswerContent { get; set; }

        [CheckAtLeastOneAttribute]
        public bool IsFirstAnswerCorrect { get; set; }

        [RequiredWithErrorMessage]
        public string SecondAnswerContent { get; set; }

        [CheckAtLeastOneAttribute]
        public bool IsSecondAnswerCorrect { get; set; }

        [RequiredWithErrorMessage]
        public string ThirdAnswerContent { get; set; }

        [CheckAtLeastOneAttribute]
        public bool IsThirdAnswerCorrect { get; set; }

        [RequiredWithErrorMessage]
        public string FourthAnswerContent { get; set; }

        [CheckAtLeastOneAttribute]
        public bool IsFourthAnswerCorrect { get; set; }
    }
}
