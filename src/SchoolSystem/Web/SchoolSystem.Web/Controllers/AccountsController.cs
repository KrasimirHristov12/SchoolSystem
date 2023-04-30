using Microsoft.AspNetCore.Mvc;

namespace SchoolSystem.Web.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Register()
        {
            return this.View();
        }
    }
}
