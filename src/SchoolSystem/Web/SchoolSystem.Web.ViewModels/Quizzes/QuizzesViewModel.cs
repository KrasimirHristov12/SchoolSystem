namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System.Collections.Generic;

    using SchoolSystem.Web.ViewModels.Classes;
    using SchoolSystem.Web.ViewModels.Subjects;

    public class QuizzesViewModel
    {
        public IEnumerable<SubjectViewModel> Subjects { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }
    }
}
