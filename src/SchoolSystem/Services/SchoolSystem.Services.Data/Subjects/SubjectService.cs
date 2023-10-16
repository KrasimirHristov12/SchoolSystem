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
        private readonly IDeletableEntityRepository<TeachersClassesSubjects> teachersClassesSubjectsRepo;

        public SubjectService(IDeletableEntityRepository<Subject> subjectsRepo, IDeletableEntityRepository<TeachersClassesSubjects> teachersClassesSubjectsRepo)
        {
            this.subjectsRepo = subjectsRepo;
            this.teachersClassesSubjectsRepo = teachersClassesSubjectsRepo;
        }

        public IEnumerable<T> GetAllTaughtForTeacher<T>(int teacherId)
        {
            return this.subjectsRepo.AllAsNoTracking().Where(s => s.TeachersClassesSubjects.Any(t => t.Id == teacherId)).To<T>().ToList();
        }

        public async Task<List<CRUDResult>> AddSubjectsToTeacherAsync(IList<int?> subjectIds, int teacherId, int classId)
        {
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

                    if (subjectIds.Where(s => s == subjectId).Count() > 1)
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.SubjectShouldBeUnique);
                    }

                    if (!this.subjectsRepo.AllAsNoTracking().Any(s => s.Id == subjectId))
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.SubjectDoesNotExist);
                    }

                    if (this.teachersClassesSubjectsRepo.AllAsNoTracking().Any(x => x.TeacherId == teacherId && x.SubjectId == subjectId && x.ClassId == classId))
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.SubjectAlreadyInTeacherList);
                    }

                    if (result.ErrorMessages.Count == 0)
                    {
                        result.Succeeded = true;
                        await this.teachersClassesSubjectsRepo.AddAsync(new TeachersClassesSubjects
                        {
                            ClassId = classId,
                            SubjectId = (int)subjectId,
                            TeacherId = teacherId,
                        });
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
                await this.teachersClassesSubjectsRepo.SaveChangesAsync();
            }

            return results;
        }

        public IEnumerable<T> GetAllSubjects<T>()
        {
            return this.subjectsRepo.AllAsNoTracking().To<T>().ToList();
        }
    }
}
