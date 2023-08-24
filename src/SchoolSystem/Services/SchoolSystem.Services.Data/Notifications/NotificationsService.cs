namespace SchoolSystem.Services.Data.Notifications
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.ViewModels.Notifications;

    public class NotificationsService : INotificationsService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationsService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<bool> AddAsync(NotificationType type, IEnumerable<string> receiversIds, string message)
        {
            var foundReceivers = new List<ApplicationUser>();

            foreach (var receiverId in receiversIds)
            {
                var foundReceiverId = this.userManager.Users.Where(u => u.Id == receiverId).FirstOrDefault();

                if (foundReceiverId != null)
                {
                    foundReceivers.Add(foundReceiverId);
                }
            }

            if (!foundReceivers.Any())
            {
                return false;
            }

            var notification = new Notification
            {
                Type = type,
                Message = message,
            };

            this.db.Notifications.Add(notification);
            foreach (var receiverId in receiversIds)
            {
                notification.Receivers.Add(new NotificationsReceivers
                {
                    NotificationId = notification.Id,
                    ReceiverId = receiverId,
                });
            }

            await this.db.SaveChangesAsync();

            return true;
        }

        public IEnumerable<NotificationViewModel> GetNotifications(string userId, bool getNewOnesOnly)
        {
            var notifications = this.db.Notifications.Where(n => n.Receivers.Any(u => u.ReceiverId == userId)).OrderByDescending(n => n.CreatedOn);
            var notificationsViewModel = new List<NotificationViewModel>();

            if (getNewOnesOnly)
            {
                notificationsViewModel = notifications.Where(n => n.IsNew).Select(n => new NotificationViewModel
                {
                    Type = n.Type,
                    Message = n.Message,
                    IsNew = n.IsNew,
                }).ToList();
            }
            else
            {
                notificationsViewModel = notifications.Select(n => new NotificationViewModel
                {
                    Type = n.Type,
                    Message = n.Message,
                    IsNew = n.IsNew,
                }).ToList();
            }

            return notificationsViewModel;
        }

        public async Task MarkNotificationsSeenAsync(string userId)
        {
            var newNotifications = this.db.Notifications.Where(n => n.Receivers.Any(u => u.ReceiverId == userId) && n.IsNew);

            foreach (var notification in newNotifications)
            {
                notification.IsNew = false;
            }

            if (newNotifications.Any())
            {
                await this.db.SaveChangesAsync();
            }
        }
    }
}
