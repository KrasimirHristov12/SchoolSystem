namespace SchoolSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
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
            model.Questions = new List<QuestionsInputModel>();
            model.Questions.Add(new QuestionsInputModel());
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
            var quiz = this.quizzesService.GetQuiz(id, studentId);
            var viewModel = quiz.Model;
            for (int i = 0; i < quiz.Model.Questions.Count; i++)
            {
                model[i].Title = quiz.Model.Questions[i].Title;
                model[i].FirstAnswerContent = quiz.Model.Questions[i].FirstAnswerContent;
                model[i].SecondAnswerContent = quiz.Model.Questions[i].SecondAnswerContent;
                model[i].ThirdAnswerContent = quiz.Model.Questions[i].ThirdAnswerContent;
                model[i].FourthAnswerContent = quiz.Model.Questions[i].FourthAnswerContent;
                model[i].Type = quiz.Model.Questions[i].Type;
            }

            viewModel.Questions = model;

            for (int i = 0; i < viewModel.Questions.Count; i++)
            {
                var listOfBools = new List<bool>
                {
                    viewModel.Questions[i].IsFirstAnswerChecked, viewModel.Questions[i].IsSecondAnswerChecked, viewModel.Questions[i].IsThirdAnswerChecked, viewModel.Questions[i].IsFourthAnswerChecked,
                };
                if (viewModel.Questions[i].Type == QuestionType.Radio && listOfBools.Count(a => a == true) > 1)
                {
                    this.ModelState.AddModelError(string.Empty, GlobalConstants.ErrorMessage.AtMostOneSelectionPossibleWhenRadio);
                }
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
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
            var model = this.quizzesService.GetReviewQuizModel(quizId, studentId);
            if (model.Count() == 0)
            {
                return this.NotFound();
            }

            return this.View(model);
        }
    }
}
