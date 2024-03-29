﻿namespace SchoolSystem.Data.Models
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
            this.Grades = new HashSet<Grade>();
            this.Quizzes = new HashSet<Quiz>();
            this.TeachersClassesSubjects = new HashSet<TeachersClassesSubjects>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Subject.SubjectMaxLength)]
        public string Name { get; set; }

        public ICollection<Grade> Grades { get; set; }

        public ICollection<Quiz> Quizzes { get; set; }

        public ICollection<TeachersClassesSubjects> TeachersClassesSubjects { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
