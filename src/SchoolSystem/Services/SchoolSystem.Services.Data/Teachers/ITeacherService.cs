namespace SchoolSystem.Services.Data.Teachers
{
    using System.Collections.Generic;

    using SchoolSystem.Web.ViewModels.Teachers;

    public interface ITeacherService
    {
        int GetTeacherIdByUserId(string userId);

        IEnumerable<TeacherViewModel> GetAllTeachers();

        TeacherInformationViewModel GetTeacherInformation(int teacherId);

        string GetUserId(int teacherId);

        string GetTeacherFullName(int teacherId);

    }
}
