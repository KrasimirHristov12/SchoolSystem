namespace SchoolSystem.Services.Data.Subjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.ViewModels;

    public class SubjectService : ISubjectService
    {
        private readonly IDeletableEntityRepository<Subject> subjectsRepo;
        private readonly IDeletableEntityRepository<Teacher> teachersRepo;

        public SubjectService(IDeletableEntityRepository<Subject> subjectsRepo, IDeletableEntityRepository<Teacher> teachersRepo)
        {
            this.subjectsRepo = subjectsRepo;
            this.teachersRepo = teachersRepo;
        }

        public IEnumerable<T> GetAllTaughtForTeacher<T>(int teacherId)
        {
            return this.subjectsRepo.AllAsNoTracking().Where(s => s.Teachers.Any(t => t.Id == teacherId)).To<T>().ToList();
        }

        public async Task<List<CRUDResult>> AddSubjectsToTeacherAsync(IList<int?> subjectIds, int teacherId)
        {
            var teacher = this.teachersRepo.All().Include(t => t.Subjects).First(t => t.Id == teacherId);
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

                    var subject = this.subjectsRepo.All().FirstOrDefault(c => c.Id == subjectId);

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
                await this.subjectsRepo.SaveChangesAsync();
            }

            return results;
        }

        public IEnumerable<T> GetAllSubjects<T>()
        {
            return this.subjectsRepo.AllAsNoTracking().To<T>().ToList();
        }
    }
}
