namespace SchoolSystem.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Models.Enums;

    public class UniqueValueAttribute : ValidationAttribute
    {
        public UniqueProperty UniqueProperty { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((int)this.UniqueProperty == 0)
            {
                string egn = value.ToString();

                var db = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

                if (db.Teachers.Any(t => t.Egn == egn) || db.Students.Any(s => s.Egn == egn))
                {
                    return new ValidationResult(GlobalConstants.ErrorMessage.EgnMustBeUnique);
                }

                return ValidationResult.Success;
            }
            else if ((int)this.UniqueProperty == 1)
            {
                string phoneNumber = value.ToString();
                var db = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
                if (db.Users.Any(u => u.PhoneNumber.Replace("+359", "0") == phoneNumber.Replace("+359", "0")))
                {
                    return new ValidationResult(GlobalConstants.ErrorMessage.PhoneMustBeUnique);
                }

                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}
