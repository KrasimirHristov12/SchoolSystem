namespace SchoolSystem.Web.ViewModels.Grades
{
    using System.Collections.Generic;

    public class DisplayGradesViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalGrades { get; set; }

        public IEnumerable<GradesViewModel> Grades { get; set; }

        public FilterGradesViewModel Filter { get; set; }

    }
}
