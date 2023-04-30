namespace SchoolSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public class Teacher
    {
        public Teacher()
        {
            this.Classes = new HashSet<SchoolClass>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.EgnLength)]
        public string Egn { get; set; }

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

        public ICollection<SchoolClass> Classes { get; set; }
    }
}
