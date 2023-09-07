namespace SchoolSystem.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Chat;

    public interface IChatService
    {
        Task<ChatViewModel> AddAsync(AddChatInputModel model);

        IEnumerable<ChatViewModel> GetChats(string senderId, string receiverId);
    }
}
