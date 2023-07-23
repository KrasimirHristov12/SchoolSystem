namespace SchoolSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StudentsQuizzes
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

    }
}
