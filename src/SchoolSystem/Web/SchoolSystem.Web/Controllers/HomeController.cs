namespace SchoolSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Home;
    using SchoolSystem.Web.WebServices;

    public class HomeController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ITeacherService teacherService;
        private readonly IUserService userService;

        public HomeController(IStudentService studentService, ITeacherService teacherService, IUserService userService)
        {
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.userService.GetUserId(this.User);
                var model = new HomePageViewModel();
                if (this.User.IsInRole(GlobalConstants.Student.StudentRoleName))
                {
                    model.StudentId = this.studentService.GetIdByUserId(userId);
                    model.TeacherId = null;
                }
                else if (this.User.IsInRole(GlobalConstants.Teacher.TeacherRoleName))
                {
                    model.StudentId = null;
                    model.TeacherId = this.teacherService.GetTeacherIdByUserId(userId);
                }

                return this.View(model);
            }

            return this.RedirectToAction("Login", "Accounts");
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
