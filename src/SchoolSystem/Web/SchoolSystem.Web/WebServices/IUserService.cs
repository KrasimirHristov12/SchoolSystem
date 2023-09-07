namespace SchoolSystem.Web.WebServices
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Accounts;
    using SchoolSystem.Web.ViewModels.Chat;

    public interface IUserService
    {
        string GetUserId(ClaimsPrincipal user);

        Task<CRUDResult> RegisterAsync(RegisterInputModel model);

        Task<bool> LoginAsync(LoginInputModel model);

        Task LogoutAsync();

        IEnumerable<UserChatViewModel> GetInfoForOnlineUsers(string excludeUserId);

        string GetFullNameById(string userId);

        Task<string> GetUsernameByIdAsync(string userId);

        string GetFullNameByUsername(string username);

        Task<string> GetUserIdByUsernameAsync(string username);
    }
}
