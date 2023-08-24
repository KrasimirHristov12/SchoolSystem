namespace SchoolSystem.Web
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.SignalR;

    public class UserIdProviderCustom : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext().User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
