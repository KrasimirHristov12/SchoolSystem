namespace SchoolSystem.Web.ViewModels.Grades
{
    using System;
    using System.Globalization;
    using AutoMapper;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.Infrastructure.ExtensionMethods;

    public class GradesViewModel : IMapFrom<Grade>, IHaveCustomMappings
    {
        public string TeacherFirstName { get; set; }

        public string TeacherLastName { get; set; }

        public string SubjectName { get; set; }

        public string CreatedOnDate { get; set; }

        public GradeReason Reason { get; set; }

        public string ReasonAsString { get; set; }

        public double Value { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
             configuration.CreateMap<Grade, GradesViewModel>()
             .ForMember(vm => vm.CreatedOnDate, opt => opt.MapFrom(dm => dm.CreatedOn.Date.ToString("d", new CultureInfo("bg-BG"))));

             configuration.CreateMap<Grade, GradesViewModel>()
                .ForMember(vm => vm.ReasonAsString, opt => opt.MapFrom(dm => dm.Reason.GetDisplayName()));
        }
    }
}
