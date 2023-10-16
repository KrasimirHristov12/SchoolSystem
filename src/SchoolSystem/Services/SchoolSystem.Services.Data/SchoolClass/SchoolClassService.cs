namespace SchoolSystem.Services.Data.SchoolClass
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Classes;

    public class SchoolClassService : ISchoolClassService
    {
        private readonly IDeletableEntityRepository<Teacher> teachersRepo;
        private readonly IDeletableEntityRepository<TeachersClasses> teachersClassesRepo;
        private readonly IDeletableEntityRepository<SchoolClass> classesRepo;
        private readonly IDeletableEntityRepository<Student> studentsRepo;

        public SchoolClassService(IDeletableEntityRepository<Teacher> teachersRepo, IDeletableEntityRepository<TeachersClasses> teachersClassesRepo,IDeletableEntityRepository<SchoolClass> classesRepo, IDeletableEntityRepository<Student> studentsRepo)
        {
            this.teachersRepo = teachersRepo;
            this.teachersClassesRepo = teachersClassesRepo;
            this.classesRepo = classesRepo;
            this.studentsRepo = studentsRepo;
        }

        public IEnumerable<T> GetAllClasses<T>()
        {
            return this.classesRepo.AllAsNoTracking().To<T>().ToList();
        }

        public IEnumerable<ClassViewModel> GetAllClassesForTeacher(int teacherId)
        {
            return this.teachersClassesRepo.AllAsNoTracking().Where(x => x.TeacherId == teacherId)
                .Select(x => new ClassViewModel
                {
                    Id = x.ClassId,
                    Name = x.Class.Name,
                }).ToList();
        }

        public IEnumerable<T> GetAllFreeClasses<T>()
        {
            var allTakenClasses = this.teachersRepo.AllAsNoTracking().Where(t => t.IsClassTeacher).Select(t => t.ClassName).ToList();

            return this.classesRepo.AllAsNoTracking().Where(c => !allTakenClasses.Contains(c.Name)).To<T>().ToList();

        }

        public string GetClassNameById(int id)
        {
            string className = this.classesRepo.AllAsNoTracking().Where(c => c.Id == id).Select(c => c.Name)
                .FirstOrDefault();

            return className;
        }

        public async Task<List<CRUDResult>> AddClassesToTeacher(IList<int?> classesIds, int teacherId)
        {
            var results = new List<CRUDResult>();
            if (!this.teachersRepo.AllAsNoTracking().Any(t => t.Id == teacherId))
            {
                results.Add(new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.TeacherDoesNotExist },
                });
            }

            foreach (var classId in classesIds)
            {
                if (classId != null)
                {
                    var result = new CRUDResult()
                    {
                        Succeeded = false,
                        ErrorMessages = new List<string>(),
                    };

                    var schoolClass = this.classesRepo.All().FirstOrDefault(c => c.Id == classId);

                    if (classesIds.Where(c => c == classId).Count() > 1)
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.ClassShouldBeUnique);
                    }

                    if (!this.classesRepo.AllAsNoTracking().Any(c => c.Id == classId))
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.ClassDoesNotExist);
                    }

                    if (this.teachersClassesRepo.AllAsNoTracking().Any(x => x.TeacherId == teacherId && x.ClassId == classId))
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.ClassAlreadyInTeacherList);
                    }

                    if (result.ErrorMessages.Count == 0)
                    {
                        result.Succeeded = true;
                        await this.teachersClassesRepo.AddAsync(new TeachersClasses
                        {
                            ClassId = (int)classId,
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
                await this.teachersClassesRepo.SaveChangesAsync();
            }

            return results;
        }

        public string GetClassNameByStudentId(int studentId)
        {
            var className = this.studentsRepo.AllAsNoTracking().Where(s => s.Id == studentId).Select(s => s.Class.Name).FirstOrDefault();
            return className;
        }
    }
}
