namespace SchoolSystem.Services.Data.Students
{
    using System.Collections.Generic;

    using SchoolSystem.Web.ViewModels.Students;

    public interface IStudentService
    {
        int GetIdByUserId(string userId);

        IEnumerable<StudentsViewModel> GetAll();

        StudentInformationViewModel GetStudentInformation(int studentId);

        IEnumerable<RankingStudentViewModel> GetStudentsRanking(int page, int countPerPage);
    }
}
