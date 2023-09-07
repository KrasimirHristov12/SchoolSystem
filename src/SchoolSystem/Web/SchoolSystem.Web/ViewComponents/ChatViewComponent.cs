namespace SchoolSystem.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Web.WebServices;

    [Authorize]
    public class ChatViewComponent : ViewComponent
    {
        private readonly IUserService userService;

        public ChatViewComponent(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.userService.GetUserId(this.UserClaimsPrincipal);
            var onlineUsers = this.userService.GetInfoForOnlineUsers(userId);
            return this.View(onlineUsers);
        }
    }
}
