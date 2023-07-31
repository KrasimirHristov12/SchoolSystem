namespace SchoolSystem.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public enum GradeReason
    {
        [Display(Name = GlobalConstants.Grade.OralReason)]
        Oral,
        [Display(Name = GlobalConstants.Grade.QuizReason)]
        Quiz,
        [Display(Name = GlobalConstants.Grade.ParticipationReason)]
        Participation,
        [Display(Name = GlobalConstants.Grade.OtherReason)]
        Other,
    }
}
