namespace SchoolSystem.Web.ViewModels.Subjects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using SchoolSystem.Common;
    using SchoolSystem.Web.ViewModels.Teachers;

    [Bind(Exclude = $"{nameof(Subjects)},{nameof(Teachers)}")]
    public class SubjectsInputModel : IValidatableObject
    {
        public IList<int?> SubjectsIds { get; set; }

        [Display(Name = GlobalConstants.Teacher.TeacherDisplay + ":")]
        public int TeacherId { get; set; }

        public IEnumerable<SubjectViewModel> Subjects { get; set; }

        public IEnumerable<TeacherViewModel> Teachers { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.SubjectsIds.Where(x => x != null).Count() == 0)
            {
                yield return new ValidationResult(GlobalConstants.ErrorMessage.AtLeastOneSubjectShouldBeSelected);
            }
        }
    }
}
