namespace SchoolSystem.Data.Models
{
    using System;

    using SchoolSystem.Data.Common.Models;

    public class TeachersClassesSubjects : IDeletableEntity, IAuditInfo
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; }

        public int ClassId { get; set; }

        public SchoolClass Class { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
