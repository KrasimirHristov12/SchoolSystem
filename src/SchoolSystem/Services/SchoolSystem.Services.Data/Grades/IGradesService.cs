namespace SchoolSystem.Services.Data.Grades
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Grades;

    public interface IGradesService
    {
        Task<CRUDResult> AddAsync(GradesInputModel model, int teacherId);

        DisplayGradesViewModel GetForStudent(int studentId, int page);

        IEnumerable<GradesViewModel> GetForStudentApi(int studentId, int page);

        IEnumerable<GradesViewModel> GetFilteredGrades(IEnumerable<int> teacherIds, IEnumerable<int> subjectIds, IEnumerable<int> reasonIds, ICollection<int> gradesIds, int date, int studentId);

        Task<bool> AddAfterQuizIsTakenAsync(int teacherId, int studentId, int subjectId, int pointsEarned, IEnumerable<string> scaleRanges);

        int GetMarkByPointsEarnedAndGradingScale(int pointsEarned, IEnumerable<string> scaleRanges);

        string GetReasonNameByReasonEnum(GradeReason grade);
    }
}
