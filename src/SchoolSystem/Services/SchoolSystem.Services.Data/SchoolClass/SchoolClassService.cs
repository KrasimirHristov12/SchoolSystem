namespace SchoolSystem.Services.Data.SchoolClass
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
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

        public async Task<CRUDResult> AddClassesToTeacher(IList<int?> classesIds, int teacherId)
        {
            var teacher = this.db.Teachers.Include(t => t.Classes).First(t => t.Id == teacherId);
            foreach (var classId in classesIds.Where(c => c.HasValue))
            {
                var schoolClass = this.db.Classes.FirstOrDefault(c => c.Id == classId);

                if (classesIds.Where(c => c == classId).Count() > 1)
                {
                    return new CRUDResult
                    {
                        Succeeded = false,
                        ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassShouldBeUnique },
                    };
                }

                if (schoolClass == null)
                {
                    return new CRUDResult
                    {
                        Succeeded = false,
                        ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassDoesNotExist },
                    };
                }

                if (teacher.Classes.Any(c => c.Id == classId))
                {
                    return new CRUDResult
                    {
                        Succeeded = false,
                        ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassAlreadyInTeacherList },
                    };
                }

                teacher.Classes.Add(schoolClass);
           }

            await this.db.SaveChangesAsync();
            return new CRUDResult
            {
                Succeeded = true,
                ErrorMessages = null,
            };
        }
    }
}
