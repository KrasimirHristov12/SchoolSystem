namespace SchoolSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Web.WebServices;

    [ApiController]
    [Route("/api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersApiController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route(nameof(GetFullName))]
        public IActionResult GetFullName(string username)
        {
            var fullName = this.userService.GetFullNameByUsername(username);
            if (fullName == null)
            {
                return this.NotFound();
            }

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
