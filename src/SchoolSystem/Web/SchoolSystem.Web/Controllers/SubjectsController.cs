namespace SchoolSystem.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Subjects;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels.Classes;
    using SchoolSystem.Web.ViewModels.Subjects;

    public class SubjectsController : HeadmastersBaseController
    {
        private readonly ISubjectService subjectService;
        private readonly ITeacherService teacherService;
        private readonly ISchoolClassService classService;

        public SubjectsController(ISubjectService subjectService, ITeacherService teacherService, ISchoolClassService classService)
        {
            this.subjectService = subjectService;
            this.teacherService = teacherService;
            this.classService = classService;
        }

        public IActionResult Add()
        {
            var model = new SubjectsInputModel
            {
                Subjects = this.subjectService.GetAllSubjects<SubjectViewModel>(),
                Teachers = this.teacherService.GetAllTeachers(),
                Classes = this.classService.GetAllClasses<ClassViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SubjectsInputModel model)
        {
            model.Subjects = this.subjectService.GetAllSubjects<SubjectViewModel>();
            model.Teachers = this.teacherService.GetAllTeachers();
            model.Classes = this.classService.GetAllClasses<ClassViewModel>();
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var results = await this.subjectService.AddSubjectsToTeacherAsync(model.SubjectsIds, model.TeacherId, model.ClassId);

            for (int i = 0; i < results.Count; i++)
            {
                if (!results[i].Succeeded)
                {
                    foreach (var errMess in results[i].ErrorMessages)
                    {
                        this.ModelState.AddModelError($"SubjectsIds[{i}]", errMess);
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
