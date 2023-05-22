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
    }
}
