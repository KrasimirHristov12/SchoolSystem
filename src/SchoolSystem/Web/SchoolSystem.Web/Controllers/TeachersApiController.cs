namespace SchoolSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels.Teachers;

    [ApiController]
    [Route("api/teachers")]
    public class TeachersApiController : ControllerBase
    {
        private readonly ITeacherService teacherService;

        public TeachersApiController(ITeacherService teacherService)
        {
            this.teacherService = teacherService;
        }

        [Route(nameof(AllTeachers))]
        public IActionResult AllTeachers(int classId)
        {
            var teachers = this.teacherService.GetAllTeacherForClass<TeacherViewModel>(classId);

            return this.Ok(teachers);
        }
    }
}
