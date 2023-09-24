namespace SchoolSystem.Services.Data.Teachers
{
    using System.Collections.Generic;
    using System.Linq;

    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Web.ViewModels.Teachers;

    public class TeacherService : ITeacherService
    {
        private readonly IDeletableEntityRepository<Teacher> teacherRepo;

        public TeacherService(IDeletableEntityRepository<Teacher> teacherRepo)
        {
            this.teacherRepo = teacherRepo;
        }

        public int GetTeacherIdByUserId(string userId)
        {
            return this.teacherRepo.AllAsNoTracking().Where(t => t.UserId == userId).Select(t => t.Id).FirstOrDefault();
        }

        public IEnumerable<TeacherViewModel> GetAllTeachers()
        {
            return this.teacherRepo.AllAsNoTracking().Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FullName = $"{t.FirstName} {t.Surname} {t.LastName}",
            }).ToList();
        }

        public TeacherInformationViewModel GetTeacherInformation(int teacherId)
        {
            var teacher = this.teacherRepo.AllAsNoTracking().Where(t => t.Id == teacherId)
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
            return this.teacherRepo.AllAsNoTracking().Where(t => t.Id == teacherId).Select(t => t.FirstName + " " + t.Surname + " " + t.LastName).FirstOrDefault();
        }

        public string GetUserId(int teacherId)
        {
            return this.teacherRepo.AllAsNoTracking().Where(t => t.Id == teacherId).Select(t => t.UserId).FirstOrDefault();
        }
    }
}
