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
    using SchoolSystem.Services.Data.GradingScale;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Grades;

    public class GradesService : IGradesService
    {
        private readonly ApplicationDbContext db;
        private readonly IGradingScaleService gradingScaleService;

        public GradesService(ApplicationDbContext db, IGradingScaleService gradingScaleService)
        {
            this.db = db;
            this.gradingScaleService = gradingScaleService;
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

        public int GetMarkByPointsEarnedAndGradingScale(int pointsEarned, IEnumerable<string> scaleRanges)
        {
            int currentMark = 2;
            int finalMark = 0;

            foreach (var scale in scaleRanges)
            {
                var minMaxValues = this.gradingScaleService.GetMinMaxPoints(scale);
                int minValue = minMaxValues[0];
                int maxValue = minMaxValues[1];
                if (Enumerable.Range(minValue, maxValue - minValue + 1).Contains(pointsEarned))
                {
                    finalMark = currentMark;
                    break;
                }

                currentMark++;
            }

            return finalMark;
        }

        public async Task<bool> AddAfterQuizIsTakenAsync(int teacherId, int studentId, int subjectId, int pointsEarned, IEnumerable<string> scaleRanges)
        {
            if (!this.db.Teachers.Any(t => t.Id == teacherId))
            {
                return false;
            }

            if (!this.db.Students.Any(s => s.Id == studentId))
            {
                return false;
            }

            if (!this.db.Subjects.Any(s => s.Id == subjectId))
            {
                return false;
            }

            var markNumber = this.GetMarkByPointsEarnedAndGradingScale(pointsEarned, scaleRanges);
            var markObj = new Grade
            {
                TeacherId = teacherId,
                StudentId = studentId,
                SubjectId = subjectId,
                Value = markNumber,
            };

            this.db.Grades.Add(markObj);

            await this.db.SaveChangesAsync();
            return true;
        }
    }
}
