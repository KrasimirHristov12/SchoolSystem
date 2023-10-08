namespace SchoolSystem.Services.Data.Contact
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Web.ViewModels.Contact;

    public class ContactService : IContactService
    {
        private readonly IDeletableEntityRepository<ContactInfo> contactRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public ContactService(IDeletableEntityRepository<ContactInfo> contactRepo, UserManager<ApplicationUser> userManager)
        {
            this.contactRepo = contactRepo;
            this.userManager = userManager;
        }

        public async Task<bool> AddAsync(ContactInputModel model)
        {
            if (!this.userManager.Users.Any(u => u.Id == model.UserId))
            {
                return false;
            }

            await this.contactRepo.AddAsync(new ContactInfo
            {
                UserId = model.UserId,
                Message = model.Message,
            });
            await this.contactRepo.SaveChangesAsync();
            return true;
        }
    }
}
