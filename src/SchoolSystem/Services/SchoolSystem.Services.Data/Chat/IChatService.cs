namespace SchoolSystem.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Chat;

    public interface IChatService
    {
        Task<T> AddAsync<T>(AddChatInputModel model);

        IEnumerable<T> GetChats<T>(string senderId, string receiverId);
    }
}
