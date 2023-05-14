namespace SchoolSystem.Web.ViewModels.Accounts
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;

    public class LoginInputModel
    {
        [RequiredWithErrorMessage]
        [EmailAddress(ErrorMessage = GlobalConstants.ErrorMessage.InvalidEmail)]
        [Display(Name = GlobalConstants.EmailAddressDisplay)]
        public string Email { get; set; }

        [RequiredWithErrorMessage]
        [StringLength(GlobalConstants.PasswordMaxLength, MinimumLength = GlobalConstants.PasswordMinLength, ErrorMessage = GlobalConstants.ErrorMessage.PasswordErrorMessage)]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.PasswordDisplay)]
        public string Password { get; set; }
    }
}
