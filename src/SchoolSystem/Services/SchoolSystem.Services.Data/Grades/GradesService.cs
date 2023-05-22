namespace SchoolSystem.Services.Data.Grades
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Grades;

    public class GradesService : IGradesService
    {
        private readonly ApplicationDbContext db;

        public GradesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<GradesViewModel> GetForStudent(int studentId)
        {
            var student = this.db.Students.Where(s => s.Id == studentId);
            var cultureInfo = new CultureInfo("bg-BG");

            var grades = this.db.Students.Where(s => s.Id == studentId).Select(s => s.Grades.Select(g => new GradesViewModel
            {
                TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName,
                Date = g.CreatedOn.Date.ToString("d", cultureInfo),
                SubjectName = g.Subject.Name,
                Value = g.Value.ToString("F2"),
            })).First().ToList();

            return grades;
        }

        public async Task<CRUDResult> AddAsync(GradesInputModel model, int teacherId)
        {
            var selectedStudent = this.db.Students.Include(s => s.Grades).FirstOrDefault(st => st.Id == model.StudentId);
            var selectedClass = this.db.Classes.Include(c => c.Students).FirstOrDefault(c => c.Id == model.ClassId);
            var selectedSubject = this.db.Subjects.FirstOrDefault(s => s.Id == model.SubjectId);
            var teacher = this.db.Teachers.Include(t => t.Subjects).First(t => t.Id == teacherId);

            if (selectedStudent == null)
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.StudentDoesNotExist },
                };
            }

            if (selectedClass == null)
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassDoesNotExist },
                };
            }

            if (selectedSubject == null)
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.SubjectDoesNotExist },
                };
            }

            if (!selectedClass.Students.Any(s => s.Id == selectedStudent.Id))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.StudentNotInClass },
                };
            }

            if (!teacher.Subjects.Any(s => s.Id == model.SubjectId))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.SubjectNotInTeacherList },
                };
            }

            selectedStudent.Grades.Add(new Grade
            {
                TeacherId = teacherId,
                SubjectId = (int)model.SubjectId,
                Value = (double)model.Value,
            });
            await this.db.SaveChangesAsync();

            return new CRUDResult { Succeeded = true, ErrorMessages = null, };
        }
    }
}
