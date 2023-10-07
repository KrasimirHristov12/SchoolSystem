namespace SchoolSystem.Services.Data.Teachers
{
    using System.Collections.Generic;

    using SchoolSystem.Web.ViewModels.Teachers;

    public interface ITeacherService
    {
        int GetTeacherIdByUserId(string userId);

        IEnumerable<TeacherViewModel> GetAllTeachers();

        T GetTeacherInformation<T>(int teacherId);

        string GetUserId(int teacherId);

        string GetTeacherFullName(int teacherId);

    }
}
