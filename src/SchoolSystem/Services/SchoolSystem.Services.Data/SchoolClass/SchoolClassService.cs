namespace SchoolSystem.Services.Data.SchoolClass
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Migrations;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Classes;

    public class SchoolClassService : ISchoolClassService
    {
        private readonly IDeletableEntityRepository<Teacher> teachersRepo;
        private readonly IDeletableEntityRepository<SchoolClass> classesRepo;
        private readonly IDeletableEntityRepository<Student> studentsRepo;

        public SchoolClassService(IDeletableEntityRepository<Teacher> teachersRepo, IDeletableEntityRepository<SchoolClass> classesRepo, IDeletableEntityRepository<Student> studentsRepo)
        {
            this.teachersRepo = teachersRepo;
            this.classesRepo = classesRepo;
            this.studentsRepo = studentsRepo;
        }

        public IEnumerable<ClassViewModel> GetAllClasses()
        {
            return this.classesRepo.AllAsNoTracking().Select(c => new ClassViewModel
            {
                Id = c.Id,
                Name = c.Name,
            });
        }

        public IEnumerable<ClassViewModel> GetAllClassesForTeacher(int teacherId)
        {
            return this.classesRepo.AllAsNoTracking().Where(c => c.Teachers.Any(t => t.Id == teacherId))
                .Select(c => new ClassViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
        }

        public IEnumerable<ClassViewModel> GetAllClassesNotForTeacher(int teacherId)
        {
            return this.classesRepo.AllAsNoTracking().Where(c => !c.Teachers.Any(t => t.Id == teacherId))
                 .Select(c => new ClassViewModel
                 {
                     Id = c.Id,
                     Name = c.Name,
                 }).ToList();
        }

        public IEnumerable<ClassViewModel> GetAllFreeClasses()
        {
            return this.classesRepo.AllAsNoTracking().Where(c => !c.Teachers.Any(t => t.ClassName == c.Name)).Select(c => new ClassViewModel
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }

        public string GetClassNameById(int id)
        {
            string className = this.classesRepo.AllAsNoTracking().Where(c => c.Id == id).Select(c => c.Name)
                .FirstOrDefault();

            return className;
        }

        public async Task<List<CRUDResult>> AddClassesToTeacher(IList<int?> classesIds, int teacherId)
        {
            var teacher = this.teachersRepo.All().Include(t => t.Classes).FirstOrDefault(t => t.Id == teacherId);

            var results = new List<CRUDResult>();

            if (teacher == null)
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

                    if (schoolClass == null)
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.ClassDoesNotExist);
                    }

                    if (teacher.Classes.Any(c => c.Id == classId))
                    {
                        result.ErrorMessages.Add(GlobalConstants.ErrorMessage.ClassAlreadyInTeacherList);
                    }

                    if (result.ErrorMessages.Count == 0)
                    {
                        result.Succeeded = true;
                        teacher.Classes.Add(schoolClass);
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
                await this.teachersRepo.SaveChangesAsync();
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
