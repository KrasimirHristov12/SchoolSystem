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
        public QuizzesInputModel()
        {
            this.ScaleRangeForPoor = "0-";
        }

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

        [Display(Name = GlobalConstants.Quiz.DurationDisplay)]
        [RequiredWithErrorMessage]
        [Range(typeof(int), GlobalConstants.Quiz.DurationMinNumberAsString, GlobalConstants.Quiz.DurationMaxNumberAsString, ErrorMessage = GlobalConstants.ErrorMessage.InvalidDurationOfQuiz)]
        public int? Duration { get; set; }

        [Display(Name = GlobalConstants.Grade.PoorDisplay)]
        [RequiredWithErrorMessage]
        [RegularExpression(GlobalConstants.Grade.ScalePattern, ErrorMessage = GlobalConstants.ErrorMessage.ScaleIncorrectFormat)]
        [PoorMinValueZero]
        [MaxGreaterThanMin]
        public string ScaleRangeForPoor { get; set; }

        [Display(Name = GlobalConstants.Grade.FairDisplay)]
        [RequiredWithErrorMessage]
        [RegularExpression(GlobalConstants.Grade.ScalePattern, ErrorMessage = GlobalConstants.ErrorMessage.ScaleIncorrectFormat)]
        [MinValueOneGreaterThanMaxOfPrev(nameof(ScaleRangeForPoor))]
        [MaxGreaterThanMin]
        public string ScaleRangeForFair { get; set; }

        [Display(Name = GlobalConstants.Grade.GoodDisplay)]
        [RequiredWithErrorMessage]
        [RegularExpression(GlobalConstants.Grade.ScalePattern, ErrorMessage = GlobalConstants.ErrorMessage.ScaleIncorrectFormat)]
        [MinValueOneGreaterThanMaxOfPrev(nameof(ScaleRangeForFair))]
        [MaxGreaterThanMin]
        public string ScaleRangeForGood { get; set; }

        [Display(Name = GlobalConstants.Grade.VeryGoodDisplay)]
        [RequiredWithErrorMessage]
        [RegularExpression(GlobalConstants.Grade.ScalePattern, ErrorMessage = GlobalConstants.ErrorMessage.ScaleIncorrectFormat)]
        [MinValueOneGreaterThanMaxOfPrev(nameof(ScaleRangeForGood))]
        [MaxGreaterThanMin]
        public string ScaleRangeForVeryGood { get; set; }

        [Display(Name = GlobalConstants.Grade.ExcellentDisplay)]
        [RequiredWithErrorMessage]
        [RegularExpression(GlobalConstants.Grade.ScalePattern, ErrorMessage = GlobalConstants.ErrorMessage.ScaleIncorrectFormat)]
        [MinValueOneGreaterThanMaxOfPrev(nameof(ScaleRangeForVeryGood))]
        [MaxGreaterThanMin]
        [MaximumEqualToTotalPoints(nameof(Questions), GlobalConstants.Question.PointsPropertyName)]
        public string ScaleRangeForExcellent { get; set; }

        public QuizzesViewModel ViewModel { get; set; }
    }
}
