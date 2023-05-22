namespace SchoolSystem.Services.Data.Grades
{
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Grades;

    public interface IGradesService
    {
        Task<CRUDResult> AddAsync(GradesInputModel model, int teacherId);
    }
}
