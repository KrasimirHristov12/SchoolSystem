namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System.Collections.Generic;

    public class AnswersViewModel
    {
        public string Question { get; set; }

        public string QuestionType { get; set; }

        public IEnumerable<string> Answers { get; set; }

        public int MyProperty { get; set; }
    }
}
