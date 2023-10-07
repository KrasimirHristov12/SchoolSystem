namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System;
    using System.Linq;

    using AutoMapper;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;

    public class DisplayQuizzesViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int StudentId { get; set; }

        public string SubjectName { get; set; }

        public string TeacherFirstName { get; set; }

        public string TeacherLastName { get; set; }

        public double DurationTotalMinutes { get; set; }

        public DateTime DateTaken { get; set; }

        public bool? IsTaken { get; set; }

        public int? Points { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            int studentId = 0;

            configuration.CreateMap<Quiz, DisplayQuizzesViewModel>()
                .ForMember(vm => vm.IsTaken, opt => opt.MapFrom(dm => dm.StudentsQuizzes.Where(q => q.StudentId == studentId).Select(q => q.IsTaken).FirstOrDefault()))
                .ForMember(vm => vm.Points, opt => opt.MapFrom(dm => dm.StudentsQuizzes.Where(q => q.StudentId == studentId).Select(q => q.Points).FirstOrDefault()))
                .ForMember(vm => vm.StudentId, opt => opt.MapFrom(dm => studentId));

        }
    }
}
