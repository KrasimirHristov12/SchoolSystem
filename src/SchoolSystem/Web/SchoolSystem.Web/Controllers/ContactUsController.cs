namespace SchoolSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.Contact;
    using SchoolSystem.Services.Email;
    using SchoolSystem.Web.ViewModels.Contact;
    using SchoolSystem.Web.WebServices;

    public class ContactUsController : Controller
    {
        private readonly IContactService contactService;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration config;
        private readonly IUserService userService;

        public ContactUsController(IContactService contactService, IEmailSender emailSender, IConfiguration config, IUserService userService)
        {
            this.contactService = contactService;
            this.emailSender = emailSender;
            this.config = config;
            this.userService = userService;
        }

        [Authorize(Roles = $"{GlobalConstants.Student.StudentRoleName},{GlobalConstants.Teacher.TeacherRoleName}")]
        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize(Roles = $"{GlobalConstants.Student.StudentRoleName},{GlobalConstants.Teacher.TeacherRoleName}")]
        [HttpPost]
        public async Task<IActionResult> Index(ContactInputModel model)
        {
            model.UserId = this.userService.GetUserId(this.User);
            var result = await this.contactService.AddAsync(model);
            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.ErrorMessage.UserDoesNotExist);

                return this.View(model);
            }

            string fullName = await this.userService.GetFullNameByUserIdAsync(model.UserId);

            await this.emailSender.SendAsync(this.config["GmailSmtp:Email"], this.config["GmailSmtp:AppPassword"], this.config["GmailSmtp:Email"], "Director", string.Format(GlobalConstants.Email.NewContactMessageSubject, fullName), model.Message);

            return this.Redirect("/");
        }
    }
}
