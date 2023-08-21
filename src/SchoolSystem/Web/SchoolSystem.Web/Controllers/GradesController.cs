namespace SchoolSystem.Web.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Data.Grades;
    using SchoolSystem.Services.Data.Notifications;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Services.Data.Subjects;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels.Grades;
    using SchoolSystem.Web.WebServices;

    [Authorize]
    public class GradesController : Controller
    {
        private readonly ISubjectService subjectService;
        private readonly ITeacherService teacherService;
        private readonly ISchoolClassService classService;
        private readonly IStudentService studentService;
        private readonly IGradesService gradesService;
        private readonly IUserService userService;

        public GradesController(ISubjectService subjectService, ITeacherService teacherService, ISchoolClassService classService, IStudentService studentService, IGradesService gradesService, IUserService userService)
        {
            this.subjectService = subjectService;
            this.teacherService = teacherService;
            this.classService = classService;
            this.studentService = studentService;
            this.gradesService = gradesService;
            this.userService = userService;
        }

        [Authorize(Roles = $"{GlobalConstants.Student.StudentRoleName}")]
        public IActionResult Index(int page = 1)
        {
            var userId = this.userService.GetUserId(this.User);
            var studentId = this.studentService.GetIdByUserId(userId);
            var grades = this.gradesService.GetForStudent(studentId, page);
            return this.View(grades);
        }

        [Authorize(Roles = $"{GlobalConstants.Teacher.TeacherRoleName}")]
        public IActionResult Add()
        {
            var userId = this.userService.GetUserId(this.User);
            var teacherId = this.teacherService.GetTeacherIdByUserId(userId);

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
            var userId = this.userService.GetUserId(this.User);
            var teacherId = this.teacherService.GetTeacherIdByUserId(userId);
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

            var result = await this.gradesService.AddAsync(model, teacherId, userId);
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
