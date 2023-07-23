namespace SchoolSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Data.Common.Models;

    public class StudentQuizzesQuestionAnswer : IDeletableEntity, IAuditInfo
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public Question Question { get; set; }

        public Guid QuestionId { get; set; }

        public bool IsFirstAnswerChecked { get; set; }

        public bool IsSecondAnswerChecked { get; set; }

        public bool IsThirdAnswerChecked { get; set; }

        public bool IsFourthAnswerChecked { get; set; }

        [Required]
        public Student Student { get; set; }

        public int StudentId { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
