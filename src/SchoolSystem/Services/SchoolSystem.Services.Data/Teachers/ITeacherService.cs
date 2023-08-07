namespace SchoolSystem.Services.Data.Teachers
{
    using System.Threading.Tasks;
    public interface ITeacherService
    {
        int GetTeacherIdByUserId(string userId);
    }
}
