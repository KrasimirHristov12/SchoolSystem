namespace SchoolSystem.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.Subjects;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.WebModels;

    public class SubjectsController : TeachersBaseController
    {
        private readonly ISubjectService subjectService;
        private readonly ITeacherService teacherService;

        public SubjectsController(ISubjectService subjectService, ITeacherService teacherService)
        {
            this.subjectService = subjectService;
            this.teacherService = teacherService;
        }

        public IActionResult Overview()
        {
            var userId = this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var teacherId = this.teacherService.GetTeacherIdByUserId(userId);
            this.ViewData["teacherId"] = teacherId;
            var taughtSubjects = this.subjectService.GetAllTaughtForTeacher(teacherId);
            return this.View(taughtSubjects);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SubjectsInputModel model)
        {
            var userId = this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var teacherId = this.teacherService.GetTeacherIdByUserId(userId);
            var result = await this.subjectService.AddSubjectToTeacherAsync((int)model.SubjectId, teacherId);
            if (!result.Succeeded)
            {
                foreach (var e in result.ErrorMessages)
                {
                    this.ModelState.AddModelError(string.Empty, e);
                }
            }

            this.TempData["SuccessfullyAdded"] = GlobalConstants.Subject.SubjectSuccessfullyAdded;
            return this.RedirectToAction(nameof(this.Overview));
        }

        public async Task<IActionResult> ValidateSubjectUniquenessToTeacherList(int? subjectId)
        {
            if (subjectId == null)
            {
                return this.Json(GlobalConstants.ErrorMessage.RequiredErrorMessage);
            }

            var userId = this.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var teacherId = this.teacherService.GetTeacherIdByUserId(userId);
            var result = await this.subjectService.ValidateSubjectUniquenessToTeacherListAsync((int)subjectId, teacherId);
            if (!result.Succeeded)
            {
                return this.Json(result.ErrorMessages.First());
            }

            return this.Json(true);
        }
    }
}
