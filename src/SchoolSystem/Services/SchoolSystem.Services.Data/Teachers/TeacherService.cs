namespace SchoolSystem.Services.Data.Teachers
{
    using System.Collections.Generic;
    using System.Linq;

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

        public IEnumerable<TeacherViewModel> GetAllTeachers()
        {
            return this.db.Teachers.Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FullName = $"{t.FirstName} {t.Surname} {t.LastName}",
            }).ToList();
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

        public string GetTeacherFullName(int teacherId)
        {
            var teacherFullName = this.db.Teachers.Where(t => t.Id == teacherId).Select(t => t.FirstName + " " + t.Surname + " " + t.LastName).FirstOrDefault();
            return teacherFullName;
        }

        public string GetUserId(int teacherId)
        {
            var userId = this.db.Teachers.Where(t => t.Id == teacherId).Select(t => t.UserId).FirstOrDefault();
            return userId;
        }
    }
}
