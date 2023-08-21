namespace SchoolSystem.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.Notifications;
    using SchoolSystem.Web.WebServices;

    public class NotificationsViewComponent : ViewComponent
    {
        private readonly INotificationsService notificationsService;
        private readonly IUserService userService;

        public NotificationsViewComponent(INotificationsService notificationsService, IUserService userService)
        {
            this.notificationsService = notificationsService;
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.userService.GetUserId(this.UserClaimsPrincipal);
            var notifications = this.notificationsService.GetNotifications(userId);
            return this.View(notifications);
        }
    }
}
