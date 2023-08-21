namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;

    using SchoolSystem.Web.ViewModels.Questions;

    public class TakeQuizViewModel
    {
        public Guid Id { get; set; }

        public List<TakeQuestionsViewModel> Questions { get; set; }

        public int Duration { get; set; }

        public int TeacherId { get; set; }

        public string TeacherUserId { get; set; }

        public string TeacherFullName { get; set; }

        public int StudentId { get; set; }

        public string StudentUserId { get; set; }

        public string StudentFullName { get; set; }

        public string StudentClassName { get; set; }

        public string QuizName { get; set; }

        public int SubjectId { get; set; }

        public DateTime DateTaken { get; set; }

        public DateTime QuizEnd => this.DateTaken.AddMinutes(this.Duration);
    }
}
