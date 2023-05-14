namespace SchoolSystem.Web.ViewModels.Accounts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;
    using SchoolSystem.Web.ViewModels.Classes;

    public class RegisterInputModel : IValidatableObject
    {
        public RegisterInputModel()
        {
            this.PhoneNumber = "+359";
        }

        [RequiredWithErrorMessage]
        [RegularExpression(GlobalConstants.EgnRegexPattern, ErrorMessage = GlobalConstants.ErrorMessage.EgnErrorMessage)]
        [StringLength(GlobalConstants.EgnPhoneLength, MinimumLength = GlobalConstants.EgnPhoneLength, ErrorMessage = GlobalConstants.ErrorMessage.EgnErrorMessage)]
        [UniqueValue(UniqueProperty = UniqueProperty.EGN)]
        [Display(Name = GlobalConstants.EgnDisplay)]
        public string Egn { get; set; }

        [RequiredWithErrorMessage]
        [EmailAddress(ErrorMessage = GlobalConstants.ErrorMessage.InvalidEmail)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = GlobalConstants.EmailAddressDisplay)]
        public string Email { get; set; }

        [RequiredWithErrorMessage]
        [RegularExpression(GlobalConstants.PhoneRegexPattern, ErrorMessage = GlobalConstants.ErrorMessage.PhoneNumberErrorMessage)]
        [StringLength(GlobalConstants.PhoneNumberMaxLength, MinimumLength = GlobalConstants.PhoneNumberMinLength, ErrorMessage = GlobalConstants.ErrorMessage.PhoneNumberErrorMessage)]
        [UniqueValue(UniqueProperty = UniqueProperty.PhoneNumber)]
        [Display(Name = GlobalConstants.PhoneNumberDisplay)]
        public string PhoneNumber { get; set; }

        [RequiredWithErrorMessage]
        [StringLength(GlobalConstants.NameMaxLength, MinimumLength = GlobalConstants.NameMinLength, ErrorMessage = GlobalConstants.ErrorMessage.FirstNameErrorMessage)]
        [Display(Name = GlobalConstants.FirstNameDisplay)]
        public string FirstName { get; set; }

        [RequiredWithErrorMessage]
        [StringLength(GlobalConstants.NameMaxLength, MinimumLength = GlobalConstants.NameMinLength, ErrorMessage = GlobalConstants.ErrorMessage.SurnameErrorMessage)]
        [Display(Name = GlobalConstants.SurnameDisplay)]
        public string Surname { get; set; }

        [RequiredWithErrorMessage]
        [StringLength(GlobalConstants.NameMaxLength, MinimumLength = GlobalConstants.NameMinLength, ErrorMessage = GlobalConstants.ErrorMessage.LastNameErrorMessage)]
        [Display(Name = GlobalConstants.LastNameDisplay)]
        public string LastName { get; set; }

        [RequiredWithErrorMessage]
        [DataType(DataType.Password)]
        [StringLength(GlobalConstants.PasswordMaxLength, MinimumLength = GlobalConstants.PasswordMinLength, ErrorMessage = GlobalConstants.ErrorMessage.PasswordErrorMessage)]
        [RegularExpression(GlobalConstants.PasswordRegexPattern, ErrorMessage = GlobalConstants.ErrorMessage.PasswordRequirementsErrorMessage)]
        [Display(Name = GlobalConstants.PasswordDisplay)]
        public string Password { get; set; }

        [RequiredWithErrorMessage]
        [Compare(nameof(Password), ErrorMessage = GlobalConstants.ErrorMessage.ComparePasswordErrorMessage)]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.ConfirmPasswordDisplay)]
        public string ConfirmPassword { get; set; }

        [Display(Name = GlobalConstants.Teacher.IsTeacherDisplay)]
        [RequiredWithErrorMessage]
        public TeacherStudent? TeacherStudent { get; set; }

        [Display(Name = GlobalConstants.Teacher.IsClassTeacherDisplay)]
        public bool IsClassTeacher { get; set; }

        [Display(Name = GlobalConstants.Teacher.TeacherClassNameDisplay)]
        public int? TeacherClassId { get; set; }

        [Display(Name = GlobalConstants.Teacher.TeacherYearsOfExperienceDisplay)]
        public int? TeacherYearsOfExperience { get; set; }

        [Display(Name = GlobalConstants.Student.StudentClassDisplay)]
        public int? StudentClassId { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }

        public IEnumerable<ClassViewModel> FreeClasses { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((int)this.TeacherStudent == 0)
            {
                if (this.StudentClassId == null)
                {
                    yield return new ValidationResult(GlobalConstants.ErrorMessage.StudentShouldHaveClass, new List<string> { nameof(this.StudentClassId) });
                }

                if (this.IsClassTeacher)
                {
                    yield return new ValidationResult(GlobalConstants.ErrorMessage.StudentCannotBeHeadTeacher, new List<string> { nameof(this.IsClassTeacher) });
                }

                if (!string.IsNullOrWhiteSpace(this.TeacherClassId.ToString()))
                {
                    yield return new ValidationResult(GlobalConstants.ErrorMessage.StudentCannotBeHeadTeacher, new List<string> { nameof(this.TeacherClassId) });
                }

                if (!string.IsNullOrWhiteSpace(this.TeacherYearsOfExperience.ToString()))
                {
                    yield return new ValidationResult(GlobalConstants.ErrorMessage.StudentCannotHaveYearsOfExperience, new List<string> { nameof(this.TeacherYearsOfExperience) });
                }
            }
            else if ((int)this.TeacherStudent == 1)
            {
                if (this.IsClassTeacher && string.IsNullOrWhiteSpace(this.TeacherClassId.ToString()))
                {
                    yield return new ValidationResult(GlobalConstants.ErrorMessage.TeacherShouldHaveClassIfHeadTeacher, new List<string> { nameof(this.TeacherClassId) });
                }

                if (!this.IsClassTeacher && !string.IsNullOrWhiteSpace(this.TeacherClassId.ToString()))
                {
                    yield return new ValidationResult(GlobalConstants.ErrorMessage.TeacherShouldBeHeadTeacherIfHaveClass, new List<string> { nameof(this.IsClassTeacher) });
                }

                if (string.IsNullOrWhiteSpace(this.TeacherYearsOfExperience.ToString()))
                {
                    yield return new ValidationResult(GlobalConstants.ErrorMessage.TeacherShouldHaveYearsOfExperience, new List<string> { nameof(this.TeacherYearsOfExperience) });
                }

                if (this.StudentClassId.ToString() != string.Empty)
                {
                    yield return new ValidationResult(GlobalConstants.ErrorMessage.TeachersShouldNotBelongToClass);
                }
            }
        }
    }
}
