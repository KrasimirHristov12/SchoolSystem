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

        Task<string> GetClassNameById(int id);

        Task<List<CRUDResult>> AddClassesToTeacher(IList<int?> classesIds, int teacherId);

        string GetClassNameByStudentId(int studentId);
    }
}
