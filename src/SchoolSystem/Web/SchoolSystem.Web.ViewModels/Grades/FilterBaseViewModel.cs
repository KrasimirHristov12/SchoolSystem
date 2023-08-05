namespace SchoolSystem.Web.ViewModels.Grades
{
    using System.Collections.Generic;

    public abstract class FilterBaseViewModel
    {
        public IEnumerable<int> EntityIds { get; set; }

        public IEnumerable<string> EntityNames { get; set; }
    }
}
