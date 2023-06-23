namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System;

    public class TakeQuizViewModel
    {
        public Guid Id { get; set; }

        public int StudentId { get; set; }

        public string Content { get; set; }

        public int Duration { get; set; }

        public DateTime DateTaken { get; set; }

        public DateTime QuizEnd => this.DateTaken.AddMinutes(this.Duration);
    }
}
