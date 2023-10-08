namespace SchoolSystem.Services.Data.Contact
{
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Contact;

    public interface IContactService
    {
        Task<bool> AddAsync(ContactInputModel model);
    }
}
