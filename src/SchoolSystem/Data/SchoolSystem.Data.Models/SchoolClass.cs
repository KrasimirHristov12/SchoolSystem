namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Models;

    public class SchoolClass : IDeletableEntity, IAuditInfo
    {
        public SchoolClass()
        {
            this.Students = new HashSet<Student>();
            this.Quizzes = new HashSet<Quiz>();
            this.TeachersClassesSubjects = new HashSet<TeachersClassesSubjects>();
            this.TeachersClasses = new HashSet<TeachersClasses>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.SchoolClass.ClassMaxLength)]
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Quiz> Quizzes { get; set; }

        public ICollection<TeachersClassesSubjects> TeachersClassesSubjects { get; set; }

        public ICollection<TeachersClasses> TeachersClasses { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
