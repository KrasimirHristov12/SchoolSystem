namespace SchoolSystem.Web.WebServices
{
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Accounts;

    public interface IUserService
    {
        Task<CRUDResult> RegisterAsync(RegisterInputModel model);

        Task<bool> LoginAsync(LoginInputModel model);

        Task LogoutAsync();
    }
}
