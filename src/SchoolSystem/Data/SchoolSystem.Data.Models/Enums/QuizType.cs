namespace SchoolSystem.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public enum QuizType
    {
        [Display(Name = GlobalConstants.Quiz.EntryLevelDisplay)]
        EntryLevel,
        [Display(Name = GlobalConstants.Quiz.MidLevelDisplay)]
        MidLevel,
        [Display(Name = GlobalConstants.Quiz.PlacementDisplay)]
        Placement,
    }
}
