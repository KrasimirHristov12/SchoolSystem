namespace SchoolSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public class SchoolClass
    {
        public SchoolClass()
        {
            this.Students = new HashSet<Student>();
            this.Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.SchoolClass.ClassMaxLength)]
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
