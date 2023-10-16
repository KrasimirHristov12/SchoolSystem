 namespace SchoolSystem.Services.Data.SchoolClass
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Classes;

    public interface ISchoolClassService
    {
        IEnumerable<T> GetAllClasses<T>();

        IEnumerable<T> GetAllFreeClasses<T>();

        IEnumerable<ClassViewModel> GetAllClassesForTeacher(int teacherId);

        string GetClassNameById(int id);

        Task<List<CRUDResult>> AddClassesToTeacher(IList<int?> classesIds, int teacherId);

        string GetClassNameByStudentId(int studentId);
    }
}
