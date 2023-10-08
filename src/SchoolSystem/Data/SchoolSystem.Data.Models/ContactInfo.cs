namespace SchoolSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Data.Common.Models;

    public class ContactInfo : IDeletableEntity, IAuditInfo
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
