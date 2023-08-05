namespace SchoolSystem.Web.ViewModels.Grades
{

    public class FilterGradesViewModel
    {
        public TeachersFilterViewModel Teachers { get; set; }

        public SubjectsFilterViewModel Subjects { get; set; }

        public ReasonsFilterViewModel Reasons { get; set; }

        public GradesFilterViewModel Grades { get; set; }

        public DatesFilterViewModel Dates { get; set; }
    }
}
