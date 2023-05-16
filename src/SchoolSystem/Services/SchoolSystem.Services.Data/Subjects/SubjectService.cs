namespace SchoolSystem.Services.Data.Subjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Subjects;

    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext db;

        public SubjectService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SubjectsViewModel> GetAllTaughtForTeacher(int teacherId)
        {
            return this.db.Subjects.Where(s => s.Teachers.Any(t => t.Id == teacherId))
                .Select(s => new SubjectsViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToList();
        }

        public async Task<IEnumerable<SubjectsViewModel>> GetAllAvailableForTeacherAsync(int teacherId)
        {
            return await this.db.Subjects.Where(s => !s.Teachers.Any(t => t.Id == teacherId))
                .Select(s => new SubjectsViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToListAsync();
        }

        public async Task<CRUDResult> AddSubjectToTeacherAsync(int subjectId, int teacherId)
        {
            var teacher = await this.db.Teachers.Include(t => t.Subjects).FirstAsync(t => t.Id == teacherId);
            var subject = await this.db.Subjects.FindAsync(subjectId);
            teacher.Subjects.Add(subject);
            await this.db.SaveChangesAsync();
            return new CRUDResult
            {
                Succeeded = true,
                ErrorMessages = null,
            };
        }

        public async Task<CRUDResult> ValidateSubjectUniquenessToTeacherListAsync(int subjectId, int teacherId)
        {
            var teacher = await this.db.Teachers.Include(t => t.Subjects).FirstAsync(t => t.Id == teacherId);
            if (teacher.Subjects.Any(s => s.Id == subjectId))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.SubjectAlreadyExistsInTeacherCollection },
                };
            }

            return new CRUDResult
            {
                Succeeded = true,
                ErrorMessages = null,
            };
        }
    }
}
