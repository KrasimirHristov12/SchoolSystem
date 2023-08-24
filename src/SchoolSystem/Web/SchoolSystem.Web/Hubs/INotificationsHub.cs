namespace SchoolSystem.Web.Hubs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Notifications;

    public interface INotificationsHub
    {
        Task SendNotifications(IEnumerable<NotificationViewModel> newNotifications);
    }
}
