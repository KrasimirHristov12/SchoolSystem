namespace SchoolSystem.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public class RequiredWithErrorMessageAttribute : RequiredAttribute
    {
        public RequiredWithErrorMessageAttribute()
        {
            this.ErrorMessage = GlobalConstants.ErrorMessage.RequiredErrorMessage;
        }
    }
}
