namespace SchoolSystem.Web.ViewModels.Teachers
{
    using System.Linq;

    using AutoMapper;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;

    public class TeacherInformationViewModel : IMapFrom<Teacher>, IHaveCustomMappings
    {
        public string FullName { get; set; }

        public int YearsOfExperience { get; set; }

        public string SubjectsTaught { get; set; }

        public string ClassesTaught { get; set; }

        public bool IsClassTeacher { get; set; }

        public string ClassName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Teacher, TeacherInformationViewModel>()
                .ForMember(vm => vm.FullName, opt => opt.MapFrom(dm => dm.FirstName + " " + dm.Surname + " " + dm.LastName))
                .ForMember(vm => vm.SubjectsTaught, opt => opt.MapFrom(dm => string.Join(", ", dm.Subjects.Select(s => s.Name).ToList())))
                .ForMember(vm => vm.ClassesTaught, opt => opt.MapFrom(dm => string.Join(", ", dm.Classes.Select(c => c.Name).ToList())));

        }
    }
}
