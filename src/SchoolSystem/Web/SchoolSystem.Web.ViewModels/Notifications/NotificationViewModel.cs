namespace SchoolSystem.Web.ViewModels.Notifications
{
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public NotificationType Type { get; set; }

        public string Message { get; set; }

        public bool IsNew { get; set; }
    }
}
