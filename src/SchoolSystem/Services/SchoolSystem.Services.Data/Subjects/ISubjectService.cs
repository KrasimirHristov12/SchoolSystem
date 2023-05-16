namespace SchoolSystem.Services.Data.Subjects
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Subjects;

    public interface ISubjectService
    {
        IEnumerable<SubjectsViewModel> GetAllTaughtForTeacher(int teacherId);

        Task<IEnumerable<SubjectsViewModel>> GetAllAvailableForTeacherAsync(int teacherId);

        Task<CRUDResult> AddSubjectToTeacherAsync(int subjectId, int teacherId);

        Task<CRUDResult> ValidateSubjectUniquenessToTeacherListAsync(int subjectId, int teacherId);
    }
}
