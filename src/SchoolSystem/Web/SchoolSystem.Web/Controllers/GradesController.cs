namespace SchoolSystem.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.Grades;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Services.Data.Subjects;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels.Grades;

    [Authorize]
    public class GradesController : Controller
    {
        private readonly ISubjectService subjectService;
        private readonly ITeacherService teacherService;
        private readonly ISchoolClassService classService;
        private readonly IStudentService studentService;
        private readonly IGradesService gradesService;

        public GradesController(ISubjectService subjectService, ITeacherService teacherService, ISchoolClassService classService, IStudentService studentService, IGradesService gradesService)
        {
            this.subjectService = subjectService;
            this.teacherService = teacherService;
            this.classService = classService;
            this.studentService = studentService;
            this.gradesService = gradesService;
        }

        [Authorize(Roles = $"{GlobalConstants.Teacher.TeacherRoleName}")]
        public async Task<IActionResult> Add()
        {
            var userId = this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var teacherId = await this.teacherService.GetTeacherIdByUserIdAsync(userId);

            var model = new GradesInputModel()
            {
                GradeModel = new AddGradeViewModel
                {
                    Classes = this.classService.GetAllClassesForTeacher(teacherId),
                    Subjects = this.subjectService.GetAllTaughtForTeacher(teacherId),
                    Students = this.studentService.GetAll(),
                },
            };
            return this.View(model);
        }

        [Authorize(Roles = $"{GlobalConstants.Teacher.TeacherRoleName}")]
        [HttpPost]
        public async Task<IActionResult> Add(GradesInputModel model)
        {
            var userId = this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var teacherId = await this.teacherService.GetTeacherIdByUserIdAsync(userId);
            model.GradeModel = new AddGradeViewModel
            {
                Classes = this.classService.GetAllClassesForTeacher(teacherId),
                Subjects = this.subjectService.GetAllTaughtForTeacher(teacherId),
                Students = this.studentService.GetAll(),
            };

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.gradesService.AddAsync(model, teacherId);
            if (!result.Succeeded)
            {
                foreach (var err in result.ErrorMessages)
                {
                    this.ModelState.AddModelError(string.Empty, err);
                }

                return this.View(model);
            }

            return this.Redirect("/");
        }
    }
}
