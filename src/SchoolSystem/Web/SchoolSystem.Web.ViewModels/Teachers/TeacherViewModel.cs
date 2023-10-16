namespace SchoolSystem.Web.ViewModels.Teachers
{
    using AutoMapper;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;

    public class TeacherViewModel : IMapFrom<Teacher>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Teacher, TeacherViewModel>()
                .ForMember(vm => vm.FullName, opt => opt.MapFrom(dm => dm.FirstName + " " + dm.Surname + " " + dm.LastName));
        }
    }
}
