namespace SchoolSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public class Student
    {
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

        [Required]
        public SchoolClass Class { get; set; }

        public int ClassId { get; set; }
    }
}
