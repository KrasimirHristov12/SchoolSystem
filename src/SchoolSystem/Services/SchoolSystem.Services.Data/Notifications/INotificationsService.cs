﻿namespace SchoolSystem.Services.Data.Notifications
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.ViewModels.Notifications;

    public interface INotificationsService
    {
        Task<bool> AddAsync(NotificationType type, IEnumerable<string> receiversIds, string message);

        IEnumerable<NotificationViewModel> GetNotifications(string userId, bool getNewOnesOnly);

        Task MarkNotificationsSeenAsync(string userId);
    }
}
