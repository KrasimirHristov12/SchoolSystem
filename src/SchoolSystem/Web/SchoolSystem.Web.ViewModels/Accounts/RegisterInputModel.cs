namespace SchoolSystem.Web.ViewModels.Accounts
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public class RegisterInputModel
    {
        [Required]
        [StringLength(GlobalConstants.EgnLength, MinimumLength = GlobalConstants.EgnLength, ErrorMessage = GlobalConstants.EgnErrorMessage)]
        [Display(Name = GlobalConstants.EgnDisplay)]
        public string Egn { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = GlobalConstants.EmailAddressDisplay)]
        public string Email { get; set; }

        [Required]
        [StringLength(GlobalConstants.NameMaxLength, MinimumLength = GlobalConstants.NameMinLength, ErrorMessage = GlobalConstants.FirstNameErrorMessage)]
        [Display(Name = GlobalConstants.FirstNameDisplay)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(GlobalConstants.NameMaxLength, MinimumLength = GlobalConstants.NameMinLength, ErrorMessage = GlobalConstants.SurnameErrorMessage)]
        [Display(Name = GlobalConstants.SurnameDisplay)]
        public string Surname { get; set; }

        [Required]
        [StringLength(GlobalConstants.NameMaxLength, MinimumLength = GlobalConstants.NameMinLength, ErrorMessage = GlobalConstants.LastNameErrorMessage)]
        [Display(Name = GlobalConstants.LastNameDisplay)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.PasswordDisplay)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = GlobalConstants.ComparePasswordErrorMessage)]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.ConfirmPasswordDisplay)]
        public string ConfirmPassword { get; set; }
    }
}
