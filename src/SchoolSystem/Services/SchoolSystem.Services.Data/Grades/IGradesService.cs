namespace SchoolSystem.Services.Data.Grades
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Grades;
    using SchoolSystem.Web.ViewModels.Quizzes;

    public interface IGradesService
    {
        Task<CRUDResult> AddAsync(GradesInputModel model, int teacherId, string userId);

        DisplayGradesViewModel<T> GetForStudent<T>(int studentId, int page);

        DisplayGradesViewModel<T> GetFilteredGrades<T>(int page, int studentId, IEnumerable<int> teacherIds = null, IEnumerable<int> subjectIds = null, IEnumerable<int> reasonIds = null, ICollection<int> gradesValues = null, int? date = null);

        Task<bool> AddAfterQuizIsTakenAsync(TakeQuizViewModel model, int pointsEarned, IEnumerable<string> scaleRanges);

        int GetMarkByPointsEarnedAndGradingScale(int pointsEarned, IEnumerable<string> scaleRanges);
    }
}
