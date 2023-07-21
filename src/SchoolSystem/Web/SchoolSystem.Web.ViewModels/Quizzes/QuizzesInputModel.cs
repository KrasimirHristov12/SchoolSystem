namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;
    using SchoolSystem.Web.ViewModels.Questions;

    public class QuizzesInputModel
    {
        [RequiredWithErrorMessage]
        [StringLength(GlobalConstants.Quiz.NameMaxLength, MinimumLength = GlobalConstants.Quiz.NameMinLength, ErrorMessage = GlobalConstants.ErrorMessage.QuizNameLengthErrorMessage)]
        [Display(Name = GlobalConstants.Quiz.NameDisplay)]
        public string Name { get; set; }

        [RequiredWithErrorMessage]
        [Display(Name = GlobalConstants.Subject.SubjectNameDisplay)]
        public int? SubjectId { get; set; }

        [RequiredWithErrorMessage]
        [Display(Name = GlobalConstants.Student.StudentClassDisplay)]
        public IEnumerable<int?> ClassesId { get; set; }

        [RequiredWithErrorMessage]
        [Display(Name = GlobalConstants.Quiz.TypeDisplay)]
        public QuizType? QuizType { get; set; }

        [RequiredWithErrorMessage]
        [Display(Name = GlobalConstants.Quiz.DateTakenDisplay)]
        public DateTime? DateTaken { get; set; }

        public List<QuestionsInputModel> Questions { get; set; }

        [RequiredWithErrorMessage]
        [Range(GlobalConstants.Quiz.DurationMinNumber, GlobalConstants.Quiz.DurationMaxNumber, ErrorMessage = GlobalConstants.ErrorMessage.InvalidDurationOfQuiz)]
        [Display(Name = GlobalConstants.Quiz.DurationDisplay)]
        public int? Duration { get; set; }

        public QuizzesViewModel ViewModel { get; set; }
    }
}
