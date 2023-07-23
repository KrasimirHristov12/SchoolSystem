namespace SchoolSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.Quizzes;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Services.Data.Subjects;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels.Questions;
    using SchoolSystem.Web.ViewModels.Quizzes;
    using SchoolSystem.Web.WebServices;

    public class QuizzesController : Controller
    {
        private readonly IUserService userService;
        private readonly ITeacherService teacherService;
        private readonly ISchoolClassService classService;
        private readonly ISubjectService subjectService;
        private readonly IQuizzesService quizzesService;
        private readonly IStudentService studentService;

        public QuizzesController(IUserService userService, ITeacherService teacherService, ISchoolClassService classService, ISubjectService subjectService,
            IQuizzesService quizzesService, IStudentService studentService)
        {
            this.userService = userService;
            this.teacherService = teacherService;
            this.classService = classService;
            this.subjectService = subjectService;
            this.quizzesService = quizzesService;
            this.studentService = studentService;
        }

        [Authorize(Roles = GlobalConstants.Student.StudentRoleName)]
        public IActionResult Mine(string date)
        {
            if (date == null)
            {
                date = DateTime.UtcNow.ToString("yyyy/MM/dd");
            }

            string userId = this.userService.GetUserId(this.User);
            int studentId = this.studentService.GetIdByUserId(userId);

            var mineQuizzes = this.quizzesService.GetMine(studentId, date);
            return this.View(mineQuizzes);
        }

        [Authorize(Roles = GlobalConstants.Teacher.TeacherRoleName)]
        public async Task<IActionResult> Add()
        {
            string userId = this.userService.GetUserId(this.User);
            int teacherId = await this.teacherService.GetTeacherIdByUserIdAsync(userId);
            var model = new QuizzesInputModel();
            model.ViewModel = new QuizzesViewModel
            {
                Subjects = this.subjectService.GetAllTaughtForTeacher(teacherId),
                Classes = this.classService.GetAllClassesForTeacher(teacherId),
            };
            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.Teacher.TeacherRoleName)]
        [HttpPost]
        public async Task<IActionResult> Add(QuizzesInputModel model)
        {
            string userId = this.userService.GetUserId(this.User);
            int teacherId = await this.teacherService.GetTeacherIdByUserIdAsync(userId);
            if (!this.ModelState.IsValid)
            {
                model.ViewModel = new QuizzesViewModel
                {
                    Subjects = this.subjectService.GetAllTaughtForTeacher(teacherId),
                    Classes = this.classService.GetAllClassesForTeacher(teacherId),
                };
                return this.View(model);
            }

            await this.quizzesService.AddAsync(model, teacherId);

            return this.Redirect("/");
        }

        [Authorize(Roles = GlobalConstants.Student.StudentRoleName)]
        public IActionResult Take(Guid id)
        {
            var userId = this.userService.GetUserId(this.User);
            var studentId = this.studentService.GetIdByUserId(userId);
            var quiz = this.quizzesService.GetQuiz(id, studentId);
            if (!quiz.IsSuccessful)
            {
                return this.Content(quiz.Message);
            }

            return this.View(quiz.Model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.Student.StudentRoleName)]
        public async Task<IActionResult> Take(Guid id, List<TakeQuestionsViewModel> model)
        {
            var userId = this.userService.GetUserId(this.User);
            var studentId = this.studentService.GetIdByUserId(userId);
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.quizzesService.RecordAnswersAsync(id, studentId, model);
            if (!result)
            {
                return this.NotFound();
            }

            return this.Redirect("/");
        }

        public IActionResult Review(Guid quizId, int studentId)
        {
            var model = this.quizzesService.GetReviewQuiz(quizId, studentId);
            if (model == null)
            {
                return this.NotFound();
            }

            model.QuizId = quizId;
            model.StudentId = studentId;

            return this.View(model);
        }
    }
}
