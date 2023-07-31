namespace SchoolSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Data.Common.Models;
    using SchoolSystem.Data.Models.Enums;

    public class Grade : IDeletableEntity, IAuditInfo
    {
        public int Id { get; set; }

        public GradeReason Reason { get; set; }

        public double Value { get; set; }

        [Required]
        public Student Student { get; set; }

        public int StudentId { get; set; }

        [Required]
        public Teacher Teacher { get; set; }

        public int TeacherId { get; set; }

        [Required]
        public Subject Subject { get; set; }

        public int SubjectId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
