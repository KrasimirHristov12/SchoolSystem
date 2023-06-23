namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System;
    using System.Collections.Generic;

    public class ResultsInputModel
    {
        public Guid QuizId { get; set; }

        public int StudentId { get; set; }

        public IEnumerable<AnswersViewModel> AnswersViewModel { get; set; }
    }
}
