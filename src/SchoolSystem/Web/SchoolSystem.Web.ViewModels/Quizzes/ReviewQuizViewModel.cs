namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;

    public class ReviewQuizViewModel
    {
        public Guid QuizId { get; set; }

        public int StudentId { get; set; }

        public string Content { get; set; }

        public string CorrectAnswers { get; set; }

        public string GivenAnswers { get; set; }
    }
}
