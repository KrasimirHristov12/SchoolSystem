﻿namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Models;

    public class Student : IDeletableEntity, IAuditInfo
    {
        public Student()
        {
            this.Grades = new HashSet<Grade>();
            this.StudentsQuizzes = new HashSet<StudentsQuizzes>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.EgnPhoneLength)]
        public string Egn { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(GlobalConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(GlobalConstants.NameMaxLength)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(GlobalConstants.NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public SchoolClass Class { get; set; }

        public ICollection<Grade> Grades { get; set; }

        public ICollection<StudentsQuizzes> StudentsQuizzes { get; set; }

        public int ClassId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
