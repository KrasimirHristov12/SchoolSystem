namespace SchoolSystem.Services.Data.SchoolClass
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Classes;

    public interface ISchoolClassService
    {
        IEnumerable<ClassViewModel> GetAllClasses();

        IEnumerable<ClassViewModel> GetAllFreeClasses();

        IEnumerable<ClassViewModel> GetAllClassesForTeacher(int teacherId);

        IEnumerable<ClassViewModel> GetAllClassesNotForTeacher(int teacherId);

        Task<string> GetClassNameById(int id);

        Task<CRUDResult> AddClassesToTeacher(IList<int?> classesIds, int teacherId);
    }
}
