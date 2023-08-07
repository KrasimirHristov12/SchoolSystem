namespace SchoolSystem.Web.ViewModels.Students
{
    public class StudentInformationViewModel
    {
        public string FullName { get; set; }

        public string ClassName { get; set; }

        public double AverageGrade { get; set; }

        public string StrongestSubjectName { get; set; }

        public double StrongestSubjectAverageGrade { get; set; }

        public string WeakestSubjectName { get; set; }

        public double WeakestSubjectAverageGrade { get; set; }
    }
}
