namespace SchoolSystem.Web.ViewModels.Answers
{
    using System.Web.Mvc;

    [Bind(Exclude = $"{nameof(Content)}")]
    public class TakeAnswersViewModel
    {
        public string Content { get; set; }

        public bool IsChecked { get; set; }
    }
}
