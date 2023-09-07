namespace SchoolSystem.Web.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using SchoolSystem.Web.Infrastructure.HubHelpers;
    using SchoolSystem.Web.ViewModels.Chat;
    using SchoolSystem.Web.WebServices;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUserService userService;

        public ChatHub(IUserService userService)
        {
            this.userService = userService;
        }

        public override async Task OnConnectedAsync()
        {
            ConnectedUser.Ids.Add(this.Context.UserIdentifier);
            string fullName = this.userService.GetFullNameById(this.Context.UserIdentifier);
            string username = await this.userService.GetUsernameByIdAsync(this.Context.UserIdentifier);
            if (fullName != null && username != null)
            {
                var userInfo = new UserChatViewModel
                {
                    FullName = fullName,
                    Username = username,
                };
                await this.Clients.Others.SendAsync("ClientConnected", userInfo);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (ConnectedUser.Ids.Contains(this.Context.UserIdentifier))
            {
                ConnectedUser.Ids.Remove(this.Context.UserIdentifier);

                string fullName = this.userService.GetFullNameById(this.Context.UserIdentifier);
                string username = await this.userService.GetUsernameByIdAsync(this.Context.UserIdentifier);

                if (fullName != null && username != null)
                {
                    var userInfo = new UserChatViewModel
                    {
                        FullName = fullName,
                        Username = username,
                    };
                    await this.Clients.Others.SendAsync("ClientDisconnected", userInfo);
                }
            }
        }

        public async Task SendMessage(string userId, string message)
        {
            string senderUsername = this.Context.User.Identity.Name;
            await this.Clients.User(userId).SendAsync("VisualizeMessage", message, senderUsername);
        }

    }
}
