namespace SchoolSystem.Web.ViewModels.Contact
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using SchoolSystem.Common;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;

    [Bind(Exclude = $"{nameof(UserId)}")]
    public class ContactInputModel
    {
        [Display(Name = GlobalConstants.Contact.MessageDisplay)]
        [RequiredWithErrorMessage]
        [MinLength(GlobalConstants.Contact.MessageMinLength, ErrorMessage = GlobalConstants.ErrorMessage.ContactMessageErrorMessage)]
        public string Message { get; set; }

        public string UserId { get; set; }
    }
}
