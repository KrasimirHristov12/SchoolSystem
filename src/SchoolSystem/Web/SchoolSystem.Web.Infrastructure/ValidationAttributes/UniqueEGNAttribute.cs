namespace SchoolSystem.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using SchoolSystem.Common;
    using SchoolSystem.Data;

    public class UniqueEGNAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string egn = value.ToString();

            var db = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            if (db.Teachers.Any(t => t.Egn == egn) || db.Students.Any(s => s.Egn == egn))
            {
                return new ValidationResult(GlobalConstants.ErrorMessage.EgnMustBeUnique);
            }

            return ValidationResult.Success;
        }
    }
}
