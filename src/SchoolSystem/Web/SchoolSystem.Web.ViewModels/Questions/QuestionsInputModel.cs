namespace SchoolSystem.Web.ViewModels.Questions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;
    using SchoolSystem.Web.ViewModels.Answers;

    public class QuestionsInputModel
    {
        [RequiredWithErrorMessage]
        [Display(Name = GlobalConstants.Question.TitleDisplay)]
        public string Title { get; set; }

        [RequiredWithErrorMessage]
        public QuestionType? QuestionType { get; set; }

        [Display(Name = GlobalConstants.Question.PointsDisplay)]
        [RequiredWithErrorMessage]
        [Range(typeof(int), GlobalConstants.Question.MinimumPointsAsString, GlobalConstants.Question.MaximumPointsAsString, ErrorMessage = GlobalConstants.ErrorMessage.PointsErrorMessage)]
        public int? Points { get; set; }

        public AnswersInputModel Answers { get; set; }
    }
}
