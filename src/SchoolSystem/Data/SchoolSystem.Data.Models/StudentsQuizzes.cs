namespace SchoolSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Data.Common.Models;

    public class StudentsQuizzes : IAuditInfo, IDeletableEntity
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        [Required]
        public Student Student { get; set; }

        public Guid QuizId { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        public bool IsTaken { get; set; }

        public int Points { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
