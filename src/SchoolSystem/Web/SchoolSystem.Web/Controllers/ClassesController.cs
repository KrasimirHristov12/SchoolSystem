namespace SchoolSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels.Classes;
    using SchoolSystem.Web.WebServices;

    public class ClassesController : HeadmastersBaseController
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
            var model = new ClassInputModel()
            {
                Teachers = this.teacherService.GetAllTeachers(),
                Classes = this.classService.GetAllClasses<ClassViewModel>(),
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClassInputModel model)
        {
            model.Classes = this.classService.GetAllClasses<ClassViewModel>();
            model.Teachers = this.teacherService.GetAllTeachers();
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var results = await this.classService.AddClassesToTeacher(model.ClassesIds, model.TeacherId);

            for (int i = 0; i < results.Count; i++)
            {
                if (!results[i].Succeeded)
                {
                    foreach (var errMess in results[i].ErrorMessages)
                    {
                        this.ModelState.AddModelError($"ClassesIds[{i}]", errMess);
                    }
                }
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            return this.Redirect("/");
        }
    }
}
