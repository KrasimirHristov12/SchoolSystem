namespace SchoolSystem.Web.ViewModels.Grades
{
    using System.Collections.Generic;

    public class DisplayGradesViewModel<T>
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalGrades { get; set; }

        public IEnumerable<T> Grades { get; set; }

        public FilterGradesViewModel Filter { get; set; }

    }
}
