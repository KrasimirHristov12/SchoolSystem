namespace SchoolSystem.Services.Data.Teachers
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Data;
    using SchoolSystem.Web.ViewModels.Teachers;

    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext db;

        public TeacherService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public int GetTeacherIdByUserId(string userId)
        {
            var teacher = this.db.Teachers.FirstOrDefault(t => t.UserId == userId);
            if (teacher == null)
            {
                return -1;
            }

            return teacher.Id;
        }

        public TeacherInformationViewModel GetTeacherInformation(int teacherId)
        {
            var teacher = this.db.Teachers.Where(t => t.Id == teacherId)
                .Select(t => new TeacherInformationViewModel
                {
                    FullName = t.FirstName + " " + t.Surname + " " + t.LastName,
                    YearsOfExperience = t.YearsOfExperience,
                    IsClassTeacher = t.IsClassTeacher,
                    ClassName = t.ClassName,
                    SubjectsTaught = string.Join(", ", t.Subjects.Select(s => s.Name).ToList()),
                    ClassesTaught = string.Join(", ", t.Classes.Select(c => c.Name).ToList()),
                }).FirstOrDefault();
            return teacher;
        }
    }
}
