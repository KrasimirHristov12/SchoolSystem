namespace SchoolSystem.Services.Data.Students
{
    using System.Collections.Generic;

    using SchoolSystem.Web.ViewModels.Students;

    public interface IStudentService
    {
        public IEnumerable<StudentsViewModel> GetAll();
    }
}
