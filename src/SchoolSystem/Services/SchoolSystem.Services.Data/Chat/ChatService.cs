namespace SchoolSystem.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.ViewModels.Chat;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<Chat> chatRepo;

        public ChatService(IDeletableEntityRepository<Chat> chatRepo)
        {
            this.chatRepo = chatRepo;
        }

        public async Task<T> AddAsync<T>(AddChatInputModel model)
        {
            var chat = new Chat
            {
                SenderId = model.SenderId,
                ReceiverId = model.ReceiverId,
                Message = model.Message,
            };

            await this.chatRepo.AddAsync(chat);
            await this.chatRepo.SaveChangesAsync();

            return this.chatRepo.AllAsNoTracking().Where(ch => ch.Id == chat.Id).To<T>().First();
        }

        public IEnumerable<T> GetChats<T>(string senderId, string receiverId)
        {
            return this.chatRepo.AllAsNoTracking().Where(ch => (ch.SenderId == senderId && ch.ReceiverId == receiverId) || (ch.SenderId == receiverId && ch.ReceiverId == senderId))
                .To<T>().ToList();
        }
    }
}
