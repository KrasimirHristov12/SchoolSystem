namespace SchoolSystem.Web.Hubs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using SchoolSystem.Services.Data.Notifications;
    using SchoolSystem.Web.ViewModels.Notifications;

    public class NotificationsHub : Hub<INotificationsHub>
    {
        public async Task SendNotifications(IEnumerable<NotificationViewModel> newNotifications)
        {
            await this.SendNotifications(newNotifications);
        }
    }
}
