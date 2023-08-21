namespace SchoolSystem.Web.ViewModels.Notifications
{
    using SchoolSystem.Data.Models.Enums;

    public class NotificationViewModel
    {
        public NotificationType Type { get; set; }

        public string Message { get; set; }
    }
}
