namespace SchoolSystem.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public enum QuestionType
    {
        [Display(Name = GlobalConstants.Question.RadioDisplay)]
        Radio,
        [Display(Name = GlobalConstants.Question.CheckboxDisplay)]
        Checkbox,
        [Display(Name = GlobalConstants.Question.TextDisplay)]
        Text,
    }
}
