namespace SchoolSystem.Web.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.Grades;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Web.WebServices;

    [ApiController]
    [Route(GlobalConstants.Grade.ApiControllerRoute)]
    public class GradesApiController : ControllerBase
    {
        private readonly IGradesService gradesService;
        private readonly IStudentService studentService;
        private readonly IUserService userService;

        public GradesApiController(IGradesService gradesService, IStudentService studentService, IUserService userService)
        {
            this.gradesService = gradesService;
            this.studentService = studentService;
            this.userService = userService;
        }

        [HttpGet]
        [Route(GlobalConstants.Grade.GetFilteredGrades)]
        [Authorize(Roles = GlobalConstants.Student.StudentRoleName)]
        public IActionResult Get([FromQuery]IEnumerable<int> teachersIds, [FromQuery] IEnumerable<int> subjectsIds, [FromQuery]IEnumerable<int> reasonsIds, [FromQuery]ICollection<int> gradesValues, [FromQuery]int? date, [FromQuery]int page)
        {
            int studentId = this.studentService.GetIdByUserId(this.userService.GetUserId(this.User));
            var g = this.gradesService.GetFilteredGrades(page, studentId, teachersIds, subjectsIds, reasonsIds, gradesValues, date);
            return this.Ok(g);
        }
    }
}
