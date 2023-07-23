namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Models;
    using SchoolSystem.Data.Models.Enums;

    public class Question : IDeletableEntity, IAuditInfo
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Question.TitleMaxLength)]
        public string Title { get; set; }

        public QuestionType Type { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Answer.ContentMaxLength)]
        public string FirstAnswerContent { get; set; }

        public bool IsFirstAnswerCorrect { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Answer.ContentMaxLength)]
        public string SecondAnswerContent { get; set; }

        public bool IsSecondAnswerCorrect { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Answer.ContentMaxLength)]
        public string ThirdAnswerContent { get; set; }

        public bool IsThirdAnswerCorrect { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Answer.ContentMaxLength)]
        public string FourthAnswerContent { get; set; }

        public bool IsFourthAnswerCorrect { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        public Guid QuizId { get; set; }

        public int Points { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
