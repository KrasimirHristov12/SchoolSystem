namespace SchoolSystem.Web.ViewModels.Classes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using SchoolSystem.Common;
    using SchoolSystem.Web.ViewModels.Teachers;

    [Bind(Exclude = $"{nameof(Classes)},{nameof(Teachers)}")]
    public class ClassInputModel : IValidatableObject
    {
        public IList<int?> ClassesIds { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }

        public IEnumerable<TeacherViewModel> Teachers { get; set; }

        [Display(Name = GlobalConstants.Teacher.TeacherDisplay + ":")]
        public int TeacherId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ClassesIds.Where(x => x != null).Count() == 0)
            {
                yield return new ValidationResult(GlobalConstants.ErrorMessage.AtLeastOneClassShouldBeSelected);
            }
        }
    }
}
