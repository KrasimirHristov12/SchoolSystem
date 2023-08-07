namespace SchoolSystem.Services.Data.Students
{
    using System.Collections.Generic;
    using System.Linq;

    using SchoolSystem.Data;
    using SchoolSystem.Web.ViewModels.Students;

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext db;

        public StudentService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<StudentsViewModel> GetAll()
        {
            return this.db.Students.Select(s => new StudentsViewModel
            {
                Id = s.Id,
                FullName = $"{s.FirstName} {s.Surname} {s.LastName}",
                ClassId = s.ClassId,
            }).ToList();
        }

        public int GetIdByUserId(string userId)
        {
            var student = this.db.Students.FirstOrDefault(s => s.UserId == userId);
            if (student == null)
            {
                return -1;
            }

            return student.Id;
        }

        public StudentInformationViewModel GetStudentInformation(int studentId)
        {
            var student = this.db.Students.Where(s => s.Id == studentId)
                .Select(s => new StudentInformationViewModel
                {
                    FullName = s.FirstName + " " + s.Surname + " " + s.LastName,
                    ClassName = s.Class.Name,
                    AverageGrade = s.Grades.Average(s => s.Value),
                    StrongestSubjectName = s.Grades.GroupBy(g => g.Subject.Name).Select(x => new { Name = x.Key, Average = x.Average(g1 => g1.Value) }).OrderByDescending(x => x.Average).Select(x => x.Name).FirstOrDefault(),
                    StrongestSubjectAverageGrade = s.Grades.GroupBy(g => g.Subject.Name).Select(x => new { Name = x.Key, Average = x.Average(g1 => g1.Value) }).OrderByDescending(x => x.Average).Select(x => x.Average).FirstOrDefault(),
                    WeakestSubjectName = s.Grades.GroupBy(g => g.Subject.Name).Select(x => new { Name = x.Key, Average = x.Average(g1 => g1.Value) }).OrderBy(x => x.Average).Select(x => x.Name).FirstOrDefault(),
                    WeakestSubjectAverageGrade = s.Grades.GroupBy(g => g.Subject.Name).Select(x => new { Name = x.Key, Average = x.Average(g1 => g1.Value) }).OrderBy(x => x.Average).Select(x => x.Average).FirstOrDefault(),

                }).FirstOrDefault();
            return student;
        }
    }
}
 