namespace SchoolSystem.Web.WebServices
{
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Accounts;
    using SchoolSystem.Web.WebModels;

    public interface IUserService
    {
        Task<RegisterResult> RegisterAsync(RegisterInputModel model);

        Task<bool> LoginAsync(LoginInputModel model);

        Task LogoutAsync();
    }
}
