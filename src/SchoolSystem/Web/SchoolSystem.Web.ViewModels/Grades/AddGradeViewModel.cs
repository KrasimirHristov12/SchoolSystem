namespace SchoolSystem.Web.ViewModels.Grades
{
    using System.Collections.Generic;

    using SchoolSystem.Web.ViewModels.Classes;
    using SchoolSystem.Web.ViewModels.Students;
    using SchoolSystem.Web.ViewModels.Subjects;

    public class AddGradeViewModel
    {
        public IEnumerable<ClassViewModel> Classes { get; set; }

        public IEnumerable<SubjectsViewModel> Subjects { get; set; }

        public IEnumerable<StudentsViewModel> Students { get; set; }
    }
}
