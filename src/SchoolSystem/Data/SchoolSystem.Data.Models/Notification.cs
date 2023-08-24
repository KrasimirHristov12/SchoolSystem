namespace SchoolSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Models;
    using SchoolSystem.Data.Models.Enums;

    public class Notification : IDeletableEntity, IAuditInfo
    {
        public Notification()
        {
            this.Receivers = new HashSet<NotificationsReceivers>();
            this.IsNew = true;
        }

        public Guid Id { get; set; }

        public NotificationType Type { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Notification.NotificationMessageMaxLength)]
        public string Message { get; set; }

        public ICollection<NotificationsReceivers> Receivers { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsNew { get; set; }
    }
}
