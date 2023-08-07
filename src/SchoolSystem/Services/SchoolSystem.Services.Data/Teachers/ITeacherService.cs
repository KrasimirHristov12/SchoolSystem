namespace SchoolSystem.Services.Data.Teachers
{
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.Teachers;

    public interface ITeacherService
    {
        int GetTeacherIdByUserId(string userId);
        TeacherInformationViewModel GetTeacherInformation(int teacherId);
    }
}
