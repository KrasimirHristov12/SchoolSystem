namespace SchoolSystem.Web.ViewModels.Teachers
{
    public class TeacherInformationViewModel
    {
        public string FullName { get; set; }

        public int YearsOfExperience { get; set; }

        public string SubjectsTaught { get; set; }

        public string ClassesTaught { get; set; }

        public bool IsClassTeacher { get; set; }

        public string ClassName { get; set; }
    }
}
