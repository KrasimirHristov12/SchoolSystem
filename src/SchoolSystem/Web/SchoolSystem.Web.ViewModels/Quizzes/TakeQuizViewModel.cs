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

        public DateTime DateTaken { get; set; }

        public DateTime QuizEnd => this.DateTaken.AddMinutes(this.Duration);
    }
}
