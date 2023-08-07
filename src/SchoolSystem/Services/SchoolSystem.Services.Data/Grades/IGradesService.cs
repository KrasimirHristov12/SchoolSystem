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

        DisplayGradesViewModel GetFilteredGrades(int page, int studentId, IEnumerable<int> teacherIds = null, IEnumerable<int> subjectIds = null, IEnumerable<int> reasonIds = null, ICollection<int> gradesValues = null, int? date = null);

        Task<bool> AddAfterQuizIsTakenAsync(int teacherId, int studentId, int subjectId, int pointsEarned, IEnumerable<string> scaleRanges);

        int GetMarkByPointsEarnedAndGradingScale(int pointsEarned, IEnumerable<string> scaleRanges);

        string GetReasonNameByReasonEnum(GradeReason grade);
    }
}
