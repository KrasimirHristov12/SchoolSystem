namespace SchoolSystem.Web.WebModels
{
    using System.Collections.Generic;

    public class RegisterResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
