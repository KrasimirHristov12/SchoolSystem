namespace SchoolSystem.Web.ViewModels.Classes
{
    using System.Collections.Generic;

    public class ClassInputModel
    {
        public IList<int?> ClassesIds { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }
    }
}
