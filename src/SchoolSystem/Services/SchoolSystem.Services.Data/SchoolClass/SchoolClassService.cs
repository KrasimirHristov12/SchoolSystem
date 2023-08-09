namespace SchoolSystem.Services.Data.SchoolClass
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Migrations;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Classes;

    public class SchoolClassService : ISchoolClassService
    {
        private readonly ApplicationDbContext db;
        private readonly ITeacherService teacherService;

        public SchoolClassService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<ClassViewModel> GetAllClasses()
        {
            return this.db.Classes.Select(c => new ClassViewModel
            {
                Id = c.Id,
                Name = c.Name,
            });
        }

        public IEnumerable<ClassViewModel> GetAllClassesForTeacher(int teacherId)
        {
            return this.db.Classes.Where(c => c.Teachers.Any(t => t.Id == teacherId))
                .Select(c => new ClassViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
        }

        public IEnumerable<ClassViewModel> GetAllClassesNotForTeacher(int teacherId)
        {
            return this.db.Classes.Where(c => !c.Teachers.Any(t => t.Id == teacherId))
                 .Select(c => new ClassViewModel
                 {
                     Id = c.Id,
                     Name = c.Name,
                 }).ToList();
        }

        public IEnumerable<ClassViewModel> GetAllFreeClasses()
        {
            return this.db.Classes.Where(c => !c.Teachers.Any(t => t.ClassName == c.Name)).Select(c => new ClassViewModel
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }

        public async Task<string> GetClassNameById(int id)
        {
            var foundClass = await this.db.Classes.FindAsync(id);
            if (foundClass == null)
            {
                return string.Empty;
            }

            return foundClass.Name;
        }

        public async Task<List<CRUDResult>> AddClassesToTeacher(IList<int?> classesIds, int teacherId)
        {
            var teacher = this.db.Teachers.Include(t => t.Classes).First(t => t.Id == teacherId);
            var results = new List<CRUDResult>();

            foreach (var classId in classesIds)
            {
                if (classId != null)
                {
                    var result = new CRUDResult()
                    {
                        Succeeded = false,
                        ErrorMessages = new List<string>(),
                    };

                    var schoolClass = this.db.Classes.FirstOrDefault(c => c.Id == classId);

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
                await this.db.SaveChangesAsync();
            }

            return results;
        }
    }
}
