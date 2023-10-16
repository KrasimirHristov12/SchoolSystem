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
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Configuration;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Data.Grades;
    using SchoolSystem.Services.Data.GradingScale;
    using SchoolSystem.Services.Data.Notifications;
    using SchoolSystem.Services.Data.Quizzes;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Services.Data.Subjects;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Services.Email;
    using SchoolSystem.Web.Hubs;
    using SchoolSystem.Web.ViewModels.Classes;
    using SchoolSystem.Web.ViewModels.Notifications;
    using SchoolSystem.Web.ViewModels.Questions;
    using SchoolSystem.Web.ViewModels.Quizzes;
    using SchoolSystem.Web.ViewModels.Subjects;
    using SchoolSystem.Web.WebServices;

    public class QuizzesController : Controller
    {
        private readonly IUserService userService;
        private readonly ITeacherService teacherService;
        private readonly ISchoolClassService classService;
        private readonly ISubjectService subjectService;
        private readonly IQuizzesService quizzesService;
        private readonly IStudentService studentService;
        private readonly IGradesService gradesService;
        private readonly IGradingScaleService gradingScaleService;
        private readonly INotificationsService notificationsService;
        private readonly IHubContext<NotificationsHub, INotificationsHub> notificationsHub;
        private readonly IEmailSender emailSender;
        private readonly ISchoolClassService schoolClassService;
        private readonly IConfiguration config;

        public QuizzesController(IUserService userService, ITeacherService teacherService, ISchoolClassService classService, ISubjectService subjectService,
            IQuizzesService quizzesService, IStudentService studentService, IGradesService gradesService, IGradingScaleService gradingScaleService, INotificationsService notificationsService, IHubContext<NotificationsHub, INotificationsHub> notificationsHub, IEmailSender emailSender,
            ISchoolClassService schoolClassService,
            IConfiguration config)
        {
            this.userService = userService;
            this.teacherService = teacherService;
            this.classService = classService;
            this.subjectService = subjectService;
            this.quizzesService = quizzesService;
            this.studentService = studentService;
            this.gradesService = gradesService;
            this.gradingScaleService = gradingScaleService;
            this.notificationsService = notificationsService;
            this.notificationsHub = notificationsHub;
            this.emailSender = emailSender;
            this.schoolClassService = schoolClassService;
            this.config = config;
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

            var mineQuizzes = this.quizzesService.GetMine<DisplayQuizzesViewModel>(studentId, date);
            return this.View(mineQuizzes);
        }

        [Authorize(Roles = GlobalConstants.Teacher.TeacherRoleName)]
        public IActionResult Add()
        {
            string userId = this.userService.GetUserId(this.User);
            int teacherId = this.teacherService.GetTeacherIdByUserId(userId);
            var model = new QuizzesInputModel();
            model.ViewModel = new QuizzesViewModel
            {
                Subjects = this.subjectService.GetAllTaughtForTeacher<SubjectViewModel>(teacherId),
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
            int teacherId = this.teacherService.GetTeacherIdByUserId(userId);
            if (!this.ModelState.IsValid)
            {
                model.ViewModel = new QuizzesViewModel
                {
                    Subjects = this.subjectService.GetAllTaughtForTeacher<SubjectViewModel>(teacherId),
                    Classes = this.classService.GetAllClassesForTeacher(teacherId),
                };
                return this.View(model);
            }

            await this.quizzesService.AddAsync(model, teacherId);

            foreach (var classId in model.ClassesIds)
            {
                if (classId != null)
                {
                    var receiverIds = this.studentService.GetUserIdsOfAllStudentsInAClass((int)classId);
                    foreach (var receiverId in receiverIds)
                    {
                        var newNotifications = this.notificationsService.GetNotifications<NotificationViewModel>(receiverId, true);
                        await this.notificationsHub.Clients.User(receiverId).SendNotifications(newNotifications);
                    }
                }
            }

            return this.Redirect("/");
        }

        [Authorize(Roles = GlobalConstants.Student.StudentRoleName)]
        public IActionResult Take(Guid id)
        {
            var userId = this.userService.GetUserId(this.User);
            var studentId = this.studentService.GetIdByUserId(userId);
            var result = this.quizzesService.GetQuiz<TakeQuizViewModel>(id, studentId);
            if (!result.IsSuccessful)
            {
                return this.Content(result.Message);
            }

            result.Model.StudentUserId = this.studentService.GetUserId(result.Model.StudentId);
            result.Model.TeacherUserId = this.teacherService.GetUserId(result.Model.TeacherId);
            result.Model.TeacherFullName = this.teacherService.GetTeacherFullName(result.Model.TeacherId);
            result.Model.StudentFullName = this.studentService.GetFullName(result.Model.StudentId);
            result.Model.StudentClassName = this.schoolClassService.GetClassNameByStudentId(result.Model.StudentId);

            return this.View(result.Model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.Student.StudentRoleName)]
        public async Task<IActionResult> Take(Guid id, List<TakeQuestionsViewModel> model)
        {
            var userId = this.userService.GetUserId(this.User);
            var studentId = this.studentService.GetIdByUserId(userId);
            var result = this.quizzesService.GetQuiz<TakeQuizViewModel>(id, studentId);

            if (!result.IsSuccessful)
            {
                return this.Content(result.Message);
            }

            result.Model.StudentUserId = userId;
            result.Model.TeacherUserId = this.teacherService.GetUserId(result.Model.TeacherId);
            result.Model.TeacherFullName = this.teacherService.GetTeacherFullName(result.Model.TeacherId);
            result.Model.StudentFullName = this.studentService.GetFullName(result.Model.StudentId);
            result.Model.StudentClassName = this.schoolClassService.GetClassNameByStudentId(result.Model.StudentId);

            var scaleRange = this.gradingScaleService.GetGradingScale(id);
            var scaleRangeAsList = new List<string>
            {
                scaleRange.ScaleRangeForPoor, scaleRange.ScaleRangeForFair, scaleRange.ScaleRangeForGood, scaleRange.ScaleRangeForVeryGood, scaleRange.ScaleRangeForExcellent,
            };
            var viewModel = result.Model;
            for (int i = 0; i < result.Model.Questions.Count; i++)
            {
                model[i].Title = result.Model.Questions[i].Title;
                model[i].FirstAnswerContent = result.Model.Questions[i].FirstAnswerContent;
                model[i].SecondAnswerContent = result.Model.Questions[i].SecondAnswerContent;
                model[i].ThirdAnswerContent = result.Model.Questions[i].ThirdAnswerContent;
                model[i].FourthAnswerContent = result.Model.Questions[i].FourthAnswerContent;
                model[i].Type = result.Model.Questions[i].Type;
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

            int pointsEarned = await this.quizzesService.RecordAnswersAsync(id, studentId, model);
            if (pointsEarned == -1)
            {
                return this.NotFound();
            }

            var gradeResult = await this.gradesService.AddAfterQuizIsTakenAsync(viewModel, pointsEarned, scaleRangeAsList);

            if (!gradeResult)
            {
                return this.NotFound();
            }

            string studentEmail = await this.userService.GetEmailByUserIdAsync(userId);
            string studentFullName = await this.userService.GetFullNameByUserIdAsync(userId);
            await this.emailSender.SendAsync(this.config["GmailSmtp:Email"].ToString(), this.config["GmailSmtp:AppPassword"].ToString(), studentEmail, studentFullName, GlobalConstants.Email.AddedGradeSubject, string.Format(GlobalConstants.Email.AddedGradeMessage, viewModel.SubjectName, viewModel.TeacherFullName));
            var newNotifications = this.notificationsService.GetNotifications<NotificationViewModel>(viewModel.TeacherUserId, true);

            await this.notificationsHub.Clients.User(viewModel.TeacherUserId).SendNotifications(newNotifications);

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
