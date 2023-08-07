namespace SchoolSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels.Classes;
    using SchoolSystem.Web.WebServices;

    public class ClassesController : TeachersBaseController
    {
        private readonly ISchoolClassService classService;
        private readonly ITeacherService teacherService;
        private readonly IUserService userService;

        public ClassesController(ISchoolClassService classService, ITeacherService teacherService, IUserService userService)
        {
            this.classService = classService;
            this.teacherService = teacherService;
            this.userService = userService;
        }

        public IActionResult Add()
        {
            var userId = this.userService.GetUserId(this.User);
            var teacherId = this.teacherService.GetTeacherIdByUserId(userId);
            var model = new ClassInputModel()
            {
                Classes = this.classService.GetAllClassesNotForTeacher(teacherId),
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClassInputModel model)
        {
            var userId = this.userService.GetUserId(this.User);
            var teacherId = this.teacherService.GetTeacherIdByUserId(userId);

            model.Classes = this.classService.GetAllClassesNotForTeacher(teacherId);

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.classService.AddClassesToTeacher(model.ClassesIds, teacherId);

            if (!result.Succeeded)
            {
                foreach (var errorMessage in result.ErrorMessages)
                {
                    this.ModelState.AddModelError(string.Empty, errorMessage);
                }

                return this.View(model);
            }

            return this.Redirect("/");
        }
    }
}
