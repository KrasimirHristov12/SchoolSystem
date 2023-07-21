namespace SchoolSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Services.Data.Quizzes;
    using SchoolSystem.Web.ViewModels.Quizzes;

    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly IQuizzesService quizzesService;

        public ResultsController(IQuizzesService quizzesService)
        {
            this.quizzesService = quizzesService;
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.Student.StudentRoleName)]
        public async Task<IActionResult> Post(ResultsInputModel model)
        {
            //var quizActualAnswers = await this.quizzesService.GetAnswersAsync(model.QuizId);
            //if (quizActualAnswers == null)
            //{
            //    return this.NotFound();
            //}
            //
            //int points = this.GetPoints(quizActualAnswers, model.AnswersViewModel);
            //
            //await this.quizzesService.RecordAsDoneAsync(model.StudentId, model.QuizId, points, model.AnswersViewModel);
            //
            //return this.Ok(new { Answers = model.AnswersViewModel, Points = points });

            return this.NotFound();
        }

        [Route(nameof(ValidateQuestion))]
        public IActionResult ValidateQuestion(string questionTitle, string questionType, string option_a, bool option_a_checked, string option_b, bool option_b_checked, string option_c, bool option_c_checked, string option_d, bool option_d_checked)
        {
            var errorsDict = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(questionTitle))
            {
                errorsDict[nameof(questionTitle)] = GlobalConstants.ErrorMessage.RequiredErrorMessage;
            }

            if (questionType != "text")
            {
                if (string.IsNullOrWhiteSpace(option_a))
                {
                    errorsDict[nameof(option_a)] = GlobalConstants.ErrorMessage.RequiredErrorMessage;
                }

                if (string.IsNullOrWhiteSpace(option_b))
                {
                    errorsDict[nameof(option_b)] = GlobalConstants.ErrorMessage.RequiredErrorMessage;
                }

                if (string.IsNullOrWhiteSpace(option_c))
                {
                    errorsDict[nameof(option_c)] = GlobalConstants.ErrorMessage.RequiredErrorMessage;
                }

                if (string.IsNullOrWhiteSpace(option_d))
                {
                    errorsDict[nameof(option_d)] = GlobalConstants.ErrorMessage.RequiredErrorMessage;
                }

                var listOfChecked = new List<bool> { option_a_checked, option_b_checked, option_c_checked, option_d_checked };

                if (listOfChecked.Where(c => !c).Count() == listOfChecked.Count)
                {
                    errorsDict["checked"] = GlobalConstants.ErrorMessage.NoAnswerSelected;
                }
            }

            return this.Ok(errorsDict);
        }

        [Route(nameof(MarkAsCorrect))]
        public async Task<IActionResult> MarkAsCorrect(Guid quizId, int studentId)
        {
            var newPoints = await this.quizzesService.MarkAsCorrectAsync(quizId, studentId);

            if (newPoints == null)
            {
                return this.NotFound();
            }

            return this.Ok(newPoints);

        }

        private int GetPoints(IEnumerable<AnswersViewModel> correctAnswers, IEnumerable<AnswersViewModel> actualAnswers)
        {
            int points = 0;

            var correctAnswersAsList = correctAnswers.ToList();

            var actualAnswersAsList = actualAnswers.ToList();

            for (int i = 0; i < correctAnswersAsList.Count; i++)
            {
                var currentQuestion = int.Parse(correctAnswersAsList[i].Question);
                if (this.CompareAnswers(actualAnswersAsList[currentQuestion - 1].Answers.ToArray(), correctAnswersAsList[i].Answers.ToArray()))
                {
                    points += 1;
                }
            }

            return points;
        }

        private bool CompareAnswers(string[] firstArray, string[] secondArray)
        {
            if (firstArray.Length != secondArray.Length)
                return false;
            for (int i = 0; i < firstArray.Length; i++)
            {
                if (firstArray[i] != secondArray[i])
                    return false;
            }
            return true;
        }
    }
}
