namespace SchoolSystem.Services.Data.Students
{
    using System.Collections.Generic;
    using System.Linq;

    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Web.ViewModels.Students;

    public class StudentService : IStudentService
    {
        private readonly IDeletableEntityRepository<Student> studentsRepo;

        public StudentService(IDeletableEntityRepository<Student> studentsRepo)
        {
            this.studentsRepo = studentsRepo;
        }

        public IEnumerable<StudentsViewModel> GetAll()
        {
            return this.studentsRepo.AllAsNoTracking().Select(s => new StudentsViewModel
            {
                Id = s.Id,
                FullName = $"{s.FirstName} {s.Surname} {s.LastName}",
                ClassId = s.ClassId,
            }).ToList();
        }

        public int GetIdByUserId(string userId)
        {
            return this.studentsRepo.AllAsNoTracking().Where(s => s.UserId == userId).Select(s => s.Id).FirstOrDefault();
        }

        public string GetUserId(int studentId)
        {
            return this.studentsRepo.AllAsNoTracking().Where(s => s.Id == studentId).Select(s => s.UserId).FirstOrDefault();
        }

        public IEnumerable<string> GetUserIdsOfAllStudentsInAClass(int classId)
        {
            return this.studentsRepo.AllAsNoTracking().Where(s => s.ClassId == classId).Select(s => s.UserId).ToList();
        }

        public StudentInformationViewModel GetStudentInformation(int studentId)
        {
            var student = this.studentsRepo.AllAsNoTracking().Where(s => s.Id == studentId)
                .Select(s => new StudentInformationViewModel
                {
                    FullName = s.FirstName + " " + s.Surname + " " + s.LastName,
                    ClassName = s.Class.Name,
                    AverageGrade = s.Grades.Count() == 0 ? 0 : s.Grades.Average(g => g.Value),
                    StrongestSubjectName = s.Grades.GroupBy(g => g.Subject.Name).Select(x => new { Name = x.Key, Average = x.Average(g1 => g1.Value) }).OrderByDescending(x => x.Average).Select(x => x.Name).FirstOrDefault(),
                    StrongestSubjectAverageGrade = s.Grades.GroupBy(g => g.Subject.Name).Select(x => new { Name = x.Key, Average = x.Average(g1 => g1.Value) }).OrderByDescending(x => x.Average).Select(x => x.Average).FirstOrDefault(),
                    WeakestSubjectName = s.Grades.GroupBy(g => g.Subject.Name).Select(x => new { Name = x.Key, Average = x.Average(g1 => g1.Value) }).OrderBy(x => x.Average).Select(x => x.Name).FirstOrDefault(),
                    WeakestSubjectAverageGrade = s.Grades.GroupBy(g => g.Subject.Name).Select(x => new { Name = x.Key, Average = x.Average(g1 => g1.Value) }).OrderBy(x => x.Average).Select(x => x.Average).FirstOrDefault(),
                }).FirstOrDefault();
            return student;
        }

        public IEnumerable<RankingStudentViewModel> GetStudentsRanking(int page, int countPerPage)
        {
            var students = this.studentsRepo.AllAsNoTracking().Select(s => new RankingStudentViewModel
            {
                FullName = $"{s.FirstName} {s.Surname} {s.LastName}",
                ClassName = s.Class.Name,
                AvgGrade = s.Grades.Average(g => g.Value),
            }).OrderByDescending(s => s.AvgGrade).Skip((page - 1) * countPerPage).Take(countPerPage).ToList();

            return students;
        }

        public string GetFullName(int studentId)
        {
            return this.studentsRepo.AllAsNoTracking().Where(s => s.Id == studentId).Select(s => s.FirstName + " " + s.Surname + " " + s.LastName).FirstOrDefault();
        }
    }
}
