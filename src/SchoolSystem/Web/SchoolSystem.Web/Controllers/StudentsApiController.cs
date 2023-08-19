namespace SchoolSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.Students;

    [ApiController]
    [Route("api/students")]
    public class StudentsApiController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentsApiController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [Route($"{GlobalConstants.Student.RankingDisplay}")]
        public IActionResult GetRanking(int currentPage)
        {
            var students = this.studentService.GetStudentsRanking(currentPage, 30);
            return this.Ok(students);
        }
    }
}
