namespace SchoolSystem.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.Subjects;
    using SchoolSystem.Web.WebModels;

    public class AddSubjectViewComponent : ViewComponent
    {
        private readonly ISubjectService subjectService;

        public AddSubjectViewComponent(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int teacherId)
        {
            var model = new SubjectsInputModel();
            var availableSubjects = await this.subjectService.GetAllAvailableForTeacherAsync(teacherId);
            model.Subjects = availableSubjects;
            return this.View(model);
        }
    }
}
