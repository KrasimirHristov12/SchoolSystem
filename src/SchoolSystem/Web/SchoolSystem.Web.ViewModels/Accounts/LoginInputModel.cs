namespace SchoolSystem.Web.ViewModels.Accounts
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;

    public class LoginInputModel
    {
        [RequiredWithErrorMessage]
        [EmailAddress]
        [Display(Name = GlobalConstants.EmailAddressDisplay)]
        public string Email { get; set; }

        [RequiredWithErrorMessage]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.PasswordDisplay)]
        public string Password { get; set; }
    }
}
