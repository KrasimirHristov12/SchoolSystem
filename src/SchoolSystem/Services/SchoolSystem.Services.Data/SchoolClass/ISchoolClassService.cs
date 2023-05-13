namespace SchoolSystem.Services.Data.SchoolClass
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Classes;

    public interface ISchoolClassService
    {
        IEnumerable<ClassViewModel> GetAllClasses();

        IEnumerable<ClassViewModel> GetAllFreeClasses();

        Task<string> GetClassNameById(int id);
    }
}
