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

        public IEnumerable<SubjectViewModel> GetAllTaughtForTeacher(int teacherId)
        {
            return this.db.Subjects.Where(s => s.Teachers.Any(t => t.Id == teacherId))
                .Select(s => new SubjectViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToList();
        }

        public async Task<IEnumerable<SubjectViewModel>> GetAllAvailableForTeacherAsync(int teacherId)
        {
            return await this.db.Subjects.Where(s => !s.Teachers.Any(t => t.Id == teacherId))
                .Select(s => new SubjectViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToListAsync();
        }

        public async Task<List<CRUDResult>> AddSubjectsToTeacherAsync(IList<int?> subjectIds, int teacherId)
        {
            var teacher = this.db.Teachers.Include(t => t.Subjects).First(t => t.Id == teacherId);
            var results = new List<CRUDResult>();

            foreach (var subjectId in subjectIds)
            {
                if (subjectId != null)
                {
                    var result = new CRUDResult()
                    {
                        Succeeded = false,
                        ErrorMessages = new List<string>(),
                    };

                    var subject = this.db.Subjects.FirstOrDefault(c => c.Id == subjectId);

                    if (subjectIds.Where(s => s == subjectId).Count() > 1)
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.SubjectShouldBeUnique);
                    }

                    if (subject == null)
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.SubjectDoesNotExist);
                    }

                    if (teacher.Subjects.Any(s => s.Id == subjectId))
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.SubjectAlreadyInTeacherList);
                    }

                    if (result.ErrorMessages.Count == 0)
                    {
                        result.Succeeded = true;
                        teacher.Subjects.Add(subject);
                    }

                    results.Add(result);
                }
                else
                {
                    results.Add(new CRUDResult() { Succeeded = true, ErrorMessages = null, });
                }
            }

            if (results.All(r => r.Succeeded))
            {
                await this.db.SaveChangesAsync();
            }

            return results;
        }

        public IEnumerable<SubjectViewModel> GetAllSubjects()
        {
            return this.db.Subjects.Select(s => new SubjectViewModel
            {
                Id = s.Id,
                Name = s.Name,
            }).ToList();
        }
    }
}
