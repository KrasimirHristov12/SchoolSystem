namespace SchoolSystem.Web.ViewModels.Questions
{
    using System.Collections.Generic;

    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;
    using SchoolSystem.Web.ViewModels.Answers;

    public class QuestionsInputModel
    {
        [RequiredWithErrorMessage]
        public string Title { get; set; }

        [RequiredWithErrorMessage]
        public QuestionType? QuestionType { get; set; }

        public AnswersInputModel Answers { get; set; }
    }
}
