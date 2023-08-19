namespace SchoolSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.Students;

    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentService studentService;

        public StudentsController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        public IActionResult Ranking()
        {
            var students = this.studentService.GetStudentsRanking(1, 30);
            return this.View(students);
        }
    }
}
