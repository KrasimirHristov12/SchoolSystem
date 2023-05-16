namespace SchoolSystem.Services.Data.Teachers
{
    using System.Threading.Tasks;
    public interface ITeacherService
    {
        Task<int> GetTeacherIdByUserIdAsync(string userId);
    }
}
