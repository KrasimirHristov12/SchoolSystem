namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Models;
    using SchoolSystem.Data.Models.Enums;

    public class Quiz : IDeletableEntity, IAuditInfo
    {
        public Quiz()
        {
            this.Classes = new HashSet<SchoolClass>();
            this.StudentsQuizzes = new HashSet<StudentsQuizzes>();
            this.Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Quiz.NameMaxLength)]
        public string Name { get; set; }

        public int SubjectId { get; set; }

        [Required]
        public Subject Subject { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<SchoolClass> Classes { get; set; }

        public ICollection<StudentsQuizzes> StudentsQuizzes { get; set; }

        public QuizType QuizType { get; set; }

        [Required]
        public Teacher Teacher { get; set; }

        public int TeacherId { get; set; }

        public DateTime DateTaken { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
