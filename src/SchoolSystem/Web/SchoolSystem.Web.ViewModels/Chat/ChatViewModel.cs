namespace SchoolSystem.Web.ViewModels.Chat
{
    using AutoMapper;
    using SchoolSystem.Services.Mapping;

    public class ChatViewModel : IMapFrom<SchoolSystem.Data.Models.Chat>, IHaveCustomMappings
    {
        public string SenderUserName { get; set; }

        public string FullName { get; set; }

        public string Message { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SchoolSystem.Data.Models.Chat, ChatViewModel>()
                .ForMember(vm => vm.FullName, opt => opt.MapFrom(dm => dm.Sender.FirstName + " " + dm.Sender.LastName));
        }
    }
}
