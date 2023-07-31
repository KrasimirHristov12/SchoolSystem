namespace SchoolSystem.Services.Data.Grades
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Grades;

    public interface IGradesService
    {
        Task<CRUDResult> AddAsync(GradesInputModel model, int teacherId);

        DisplayGradesViewModel GetForStudent(int studentId, int page);

        Task<bool> AddAfterQuizIsTakenAsync(int teacherId, int studentId, int subjectId, int pointsEarned, IEnumerable<string> scaleRanges);

        int GetMarkByPointsEarnedAndGradingScale(int pointsEarned, IEnumerable<string> scaleRanges);
    }
}
