namespace SchoolSystem.Services.Data.Notifications
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.ViewModels.Notifications;

    public class NotificationsService : INotificationsService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Notification> notificationsRepo;

        public NotificationsService(UserManager<ApplicationUser> userManager, IDeletableEntityRepository<Notification> notificationsRepo)
        {
            this.userManager = userManager;
            this.notificationsRepo = notificationsRepo;
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

            await this.notificationsRepo.AddAsync(notification);
            foreach (var receiverId in receiversIds)
            {
                notification.Receivers.Add(new NotificationsReceivers
                {
                    NotificationId = notification.Id,
                    ReceiverId = receiverId,
                });
            }

            await this.notificationsRepo.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> GetNotifications<T>(string userId, bool getNewOnesOnly, int? page = null, int? elementsPerPage = null)
        {
            var notifications = this.notificationsRepo.AllAsNoTracking().Where(n => n.Receivers.Any(u => u.ReceiverId == userId)).OrderByDescending(n => n.CreatedOn)
                .AsQueryable();
            var notificationsViewModel = new List<T>();

            if (page != null && elementsPerPage != null)
            {
                notifications = notifications.Skip(((int)page - 1) * (int)elementsPerPage).Take((int)elementsPerPage);
            }

            if (getNewOnesOnly)
            {
                notificationsViewModel = notifications.Where(n => n.IsNew).To<T>().ToList();
            }
            else
            {
                notificationsViewModel = notifications.To<T>().ToList();
            }

            return notificationsViewModel;
        }

        public async Task MarkNotificationsSeenAsync(string userId)
        {
            var newNotifications = this.notificationsRepo.All().Where(n => n.Receivers.Any(u => u.ReceiverId == userId) && n.IsNew);

            foreach (var notification in newNotifications)
            {
                notification.IsNew = false;
            }

            if (newNotifications.Any())
            {
                await this.notificationsRepo.SaveChangesAsync();
            }
        }
    }
}
