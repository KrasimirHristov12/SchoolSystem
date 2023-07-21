namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;

    public class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }

        public Guid Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Question.TitleMaxLength)]
        public string Title { get; set; }

        public QuestionType Type { get; set; }

        public ICollection<Answer> Answers { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        public Guid QuizId { get; set; }

        public int Points { get; set; }

    }
}
