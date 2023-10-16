namespace SchoolSystem.Services.Data.Subjects
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;

    public interface ISubjectService
    {
        IEnumerable<T> GetAllSubjects<T>();

        IEnumerable<T> GetAllTaughtForTeacher<T>(int teacherId);

        Task<List<CRUDResult>> AddSubjectsToTeacherAsync(IList<int?> subjectIds, int teacherId, int classId);

    }
}
