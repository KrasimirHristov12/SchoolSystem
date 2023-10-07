namespace SchoolSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Models;

    public class Chat : IDeletableEntity, IAuditInfo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Chat.MessageMaxLength)]
        public string Message { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public ApplicationUser Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public ApplicationUser Receiver { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
