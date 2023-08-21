namespace SchoolSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NotificationsReceivers
    {
        public int Id { get; set; }

        public Guid NotificationId { get; set; }

        [Required]
        public Notification Notification { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public ApplicationUser Receiver { get; set; }
    }
}
