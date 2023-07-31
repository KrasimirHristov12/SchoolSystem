namespace SchoolSystem.Web.ViewModels.Grades
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;

    public class GradesInputModel
    {
        [Display(Name = GlobalConstants.Subject.SubjectNameDisplay)]
        [RequiredWithErrorMessage]
        public int? SubjectId { get; set; }

        [Display(Name = GlobalConstants.Student.StudentClassDisplay)]
        [RequiredWithErrorMessage]
        public int? ClassId { get; set; }

        [Display(Name = GlobalConstants.Student.StudentDisplay)]
        [RequiredWithErrorMessage]
        public int? StudentId { get; set; }

        [Display(Name = GlobalConstants.Grade.ReasonDisplay)]
        [RequiredWithErrorMessage]
        public GradeReason? Reason { get; set; }

        [Display(Name = GlobalConstants.Grade.GradeDisplay)]
        [RequiredWithErrorMessage]
        [Range(GlobalConstants.Grade.GradeMinimum, GlobalConstants.Grade.GradeMaximum, ErrorMessage = GlobalConstants.ErrorMessage.GradeErrorMessage)]
        public double? Value { get; set; }

        public AddGradeViewModel GradeModel { get; set; }
    }
}
