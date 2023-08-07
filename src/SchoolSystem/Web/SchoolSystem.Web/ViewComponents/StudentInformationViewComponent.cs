namespace SchoolSystem.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.Students;

    public class StudentInformationViewComponent : ViewComponent
    {
        private readonly IStudentService studentService;

        public StudentInformationViewComponent(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? studentId)
        {
            var studentInfo = this.studentService.GetStudentInformation((int)studentId);
            return this.View(studentInfo);
        }
    }
}
