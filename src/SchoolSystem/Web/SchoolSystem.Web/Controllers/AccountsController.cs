namespace SchoolSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Web.ViewModels.Accounts;
    using SchoolSystem.Web.WebServices;

    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IUserService userService;
        private readonly ISchoolClassService classService;

        public AccountsController(ApplicationDbContext db, IUserService userService, ISchoolClassService classService)
        {
            this.db = db;
            this.userService = userService;
            this.classService = classService;
        }

        public IActionResult Register()
        {
            var model = new RegisterInputModel
            {
                Classes = this.classService.GetAllClasses(),
                FreeClasses = this.classService.GetAllFreeClasses(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            model.Classes = this.classService.GetAllClasses();
            model.FreeClasses = this.classService.GetAllFreeClasses();

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

            return this.RedirectToAction(nameof(this.Login));
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
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

            return this.Redirect("/");
        }
    }
}
