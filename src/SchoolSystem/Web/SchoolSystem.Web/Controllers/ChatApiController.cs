namespace SchoolSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.Chat;
    using SchoolSystem.Web.ViewModels.Chat;

    [ApiController]
    [Route("api/chat")]
    public class ChatApiController : ControllerBase
    {
        private readonly IChatService chatService;

        public ChatApiController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]AddChatInputModel model)
        {
           var addedChat = await this.chatService.AddAsync<ChatViewModel>(model);
           return this.Ok(addedChat);
        }

        [HttpGet]
        public IActionResult GetChats([FromQuery]string senderId, [FromQuery]string receiverId)
        {
            var chats = this.chatService.GetChats<ChatViewModel>(senderId, receiverId);
            if (chats.Count() == 0)
            {
                return this.NotFound();
            }

            return this.Ok(chats);
        }
    }
}
