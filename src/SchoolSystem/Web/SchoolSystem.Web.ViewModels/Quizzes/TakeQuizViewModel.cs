namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.ViewModels.Questions;

    public class TakeQuizViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public List<TakeQuestionsViewModel> Questions { get; set; }

        public double DurationTotalMinutes { get; set; }

        public int TeacherId { get; set; }

        public string TeacherUserId { get; set; }

        public string TeacherFullName { get; set; }

        public int StudentId { get; set; }

        public string StudentUserId { get; set; }

        public string StudentFullName { get; set; }

        public string StudentClassName { get; set; }

        public string Name { get; set; }

        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public DateTime DateTaken { get; set; }

        public DateTime QuizEnd => this.DateTaken.AddMinutes(this.DurationTotalMinutes);

        public void CreateMappings(IProfileExpression configuration)
        {
            int studentId = 0;
            configuration.CreateMap<Quiz, TakeQuizViewModel>()
                .ForMember(vm => vm.StudentId, opt => opt.MapFrom(dm => studentId));

        }
    }
}
