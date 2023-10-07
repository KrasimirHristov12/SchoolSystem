namespace SchoolSystem.Web.ViewModels.Students
{
    using System.Linq;

    using AutoMapper;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;

    public class RankingStudentViewModel : IMapFrom<Student>, IHaveCustomMappings
    {
        public string FullName { get; set; }

        public string ClassName { get; set; }

        public double AvgGrade { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Student, RankingStudentViewModel>()
                .ForMember(vm => vm.FullName, opt => opt.MapFrom(dm => dm.FirstName + " " + dm.Surname + " " + dm.LastName))
                .ForMember(vm => vm.AvgGrade, opt => opt.MapFrom(dm => dm.Grades.Average(g => g.Value)));
        }
    }
}
