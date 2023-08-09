namespace SchoolSystem.Services.Data.Subjects
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Subjects;

    public interface ISubjectService
    {
        IEnumerable<SubjectViewModel> GetAllSubjects();

        IEnumerable<SubjectViewModel> GetAllTaughtForTeacher(int teacherId);

        Task<IEnumerable<SubjectViewModel>> GetAllAvailableForTeacherAsync(int teacherId);

        Task<List<CRUDResult>> AddSubjectsToTeacherAsync(IList<int?> subjectIds, int teacherId);

    }
}
