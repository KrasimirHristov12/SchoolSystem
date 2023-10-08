namespace SchoolSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Email;
    using SchoolSystem.Web.ViewModels.Accounts;
    using SchoolSystem.Web.ViewModels.Classes;
    using SchoolSystem.Web.WebServices;

    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IUserService userService;
        private readonly ISchoolClassService classService;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration config;

        public AccountsController(ApplicationDbContext db, IUserService userService, ISchoolClassService classService, IEmailSender emailSender, IConfiguration config)
        {
            this.db = db;
            this.userService = userService;
            this.classService = classService;
            this.emailSender = emailSender;
            this.config = config;
        }

        public IActionResult Register()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                var model = new RegisterInputModel
                {
                    Classes = this.classService.GetAllClasses<ClassViewModel>(),
                    FreeClasses = this.classService.GetAllFreeClasses<ClassViewModel>(),
                };

                return this.View(model);
            }

            return this.Forbid();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                model.Classes = this.classService.GetAllClasses<ClassViewModel>();
                model.FreeClasses = this.classService.GetAllFreeClasses<ClassViewModel>();

                if (!this.ModelState.IsValid)
                {
                    return this.View(model);
                }

                var result = await this.userService.RegisterAsync(model);
                if (!result.Succeeded)
                {
                    foreach (var e in result.ErrorMessages)
                    {
                        this.ModelState.AddModelError(string.Empty, e);
                    }

                    return this.View(model);
                }

                await this.emailSender.SendAsync(this.config["GmailSmtp:Email"].ToString(), this.config["GmailSmtp:AppPassword"].ToString(), model.Email, model.FirstName + " " + model.LastName,  GlobalConstants.Email.SuccessfullyRegisteredSubject,
                    string.Format(GlobalConstants.Email.SuccessfullyRegisteredMessage, model.FirstName));

                return this.RedirectToAction(nameof(this.Login));
            }

            return this.Forbid();
        }

        public IActionResult Login()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View();
            }

            return this.Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model, string ReturnUrl)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(model);
                }

                bool result = await this.userService.LoginAsync(model);
                if (!result)
                {
                    this.ModelState.AddModelError(string.Empty, GlobalConstants.ErrorMessage.LoginErrorMessage);
                    return this.View(model);
                }

                if (!string.IsNullOrEmpty(ReturnUrl) && this.Url.IsLocalUrl(ReturnUrl))
                {
                    return this.Redirect(ReturnUrl);
                }

                return this.Redirect("/");
            }

            return this.Forbid();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.userService.LogoutAsync();
            return this.Redirect("/");
        }
    }
}
