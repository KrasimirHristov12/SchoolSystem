namespace SchoolSystem.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Web.ViewModels.Chat;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<Chat> chatRepo;

        public ChatService(IDeletableEntityRepository<Chat> chatRepo)
        {
            this.chatRepo = chatRepo;
        }

        public async Task<ChatViewModel> AddAsync(AddChatInputModel model)
        {
            var chat = new Chat
            {
                SenderId = model.SenderId,
                ReceiverId = model.ReceiverId,
                Message = model.Message,
            };

            await this.chatRepo.AddAsync(chat);
            await this.chatRepo.SaveChangesAsync();

            return this.chatRepo.AllAsNoTracking().Where(ch => ch.Id == chat.Id).Select(ch => new ChatViewModel
            {
                FullName = ch.Sender.FirstName + " " + ch.Sender.LastName,
                Username = ch.Sender.UserName,
                Message = ch.Message,
            }).First();
        }

        public IEnumerable<ChatViewModel> GetChats(string senderId, string receiverId)
        {
            var chats = this.chatRepo.AllAsNoTracking().Where(ch => (ch.SenderId == senderId && ch.ReceiverId == receiverId) || (ch.SenderId == receiverId && ch.ReceiverId == senderId))
                .Select(ch => new ChatViewModel
                {
                    FullName = ch.Sender.FirstName + " " + ch.Sender.LastName,
                    Username = ch.Sender.UserName,
                    Message = ch.Message,
                }).ToList();

            return chats;
        }
    }
}
