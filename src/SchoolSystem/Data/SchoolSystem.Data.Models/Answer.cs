namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public class Answer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Answer.ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public Question Question { get; set; }

        public Guid QuestionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}