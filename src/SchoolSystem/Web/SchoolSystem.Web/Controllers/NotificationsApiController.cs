namespace SchoolSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.Notifications;
    using SchoolSystem.Web.WebServices;

    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsApiController : ControllerBase
    {
        private readonly INotificationsService notificationsService;
        private readonly IUserService userService;

        public NotificationsApiController(INotificationsService notificationsService, IUserService userService)
        {
            this.notificationsService = notificationsService;
            this.userService = userService;
        }

        [HttpPut($"{nameof(MarkNotificationsAsRead)}")]
        public async Task<IActionResult> MarkNotificationsAsRead()
        {
            var userId = this.userService.GetUserId(this.User);
            await this.notificationsService.MarkNotificationsSeenAsync(userId);
            return this.Ok();
        }

        [HttpGet($"{nameof(NewNotifications)}")]
        public IActionResult NewNotifications()
        {
            var userId = this.userService.GetUserId(this.User);
            var newNotifications = this.notificationsService.GetNotifications(userId, true);
            return this.Ok(newNotifications);
        }
    }
}
