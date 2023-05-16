namespace SchoolSystem.Web.ViewModels
{
    using System.Collections.Generic;

    public class CRUDResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
