namespace SchoolSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Web.WebServices;

    [ApiController]
    [Route("/api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersApiController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        [Route(nameof(GetFullName))]
        public async Task<IActionResult> GetFullName(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                return this.NotFound();
            }

            var fullName = user.FirstName + " " + user.LastName;

            return this.Ok(fullName);
        }

        [Route(nameof(GetUserId))]
        public async Task<IActionResult> GetUserId(string username)
        {
            var userId = await this.userService.GetUserIdByUsernameAsync(username);
            if (userId == null)
            {
                return this.NotFound();
            }

            return this.Ok(userId);
        }
    }
}
