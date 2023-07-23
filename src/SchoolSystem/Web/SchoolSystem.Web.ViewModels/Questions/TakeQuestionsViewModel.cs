namespace SchoolSystem.Web.ViewModels.Questions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.ViewModels.Answers;

    [Bind(Exclude = $"{nameof(Title)},{nameof(Points)},{nameof(FirstAnswerContent)},{nameof(SecondAnswerContent)},${nameof(ThirdAnswerContent)},{nameof(FourthAnswerContent)}")]
    public class TakeQuestionsViewModel : IValidatableObject
    {
        public Guid Id { get; set; }

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

        public int Points { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var listOfBools = new List<bool>
            {
                this.IsFirstAnswerChecked, this.IsSecondAnswerChecked, this.IsThirdAnswerChecked, this.IsFourthAnswerChecked,
            };
            if (this.Type == QuestionType.Radio && listOfBools.Count(a => a == true) > 1)
            {
                yield return new ValidationResult(GlobalConstants.ErrorMessage.AtMostOneSelectionPossibleWhenRadio);
            }
        }
    }
}
