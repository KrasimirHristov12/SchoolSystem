namespace SchoolSystem.Web.WebServices
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Accounts;

    public interface IUserService
    {
        string GetUserId(ClaimsPrincipal user);

        Task<CRUDResult> RegisterAsync(RegisterInputModel model);

        Task<bool> LoginAsync(LoginInputModel model);

        Task LogoutAsync();
    }
}
