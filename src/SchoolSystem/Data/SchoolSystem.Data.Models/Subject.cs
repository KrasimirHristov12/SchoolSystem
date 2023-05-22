namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Models;

    public class Subject : IDeletableEntity, IAuditInfo
    {
        public Subject()
        {
            this.Teachers = new HashSet<Teacher>();
            this.Grades = new HashSet<Grade>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Subject.SubjectMaxLength)]
        public string Name { get; set; }

        public ICollection<Teacher> Teachers { get; set; }

        public ICollection<Grade> Grades { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
