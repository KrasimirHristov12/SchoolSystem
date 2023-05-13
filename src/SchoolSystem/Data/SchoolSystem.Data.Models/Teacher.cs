namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Models;

    public class Teacher : IDeletableEntity, IAuditInfo
    {
        public Teacher()
        {
            this.Classes = new HashSet<SchoolClass>();
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

        public int YearsOfExperience { get; set; }

        public bool IsClassTeacher { get; set; }

        public string ClassName { get; set; }

        public ICollection<SchoolClass> Classes { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
