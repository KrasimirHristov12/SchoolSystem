namespace SchoolSystem.Services.Data.SchoolClass
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SchoolSystem.Data;
    using SchoolSystem.Web.ViewModels.Classes;

    public class SchoolClassService : ISchoolClassService
    {
        private readonly ApplicationDbContext db;

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

        public IEnumerable<ClassViewModel> GetAllFreeClasses()
        {
            return this.db.Classes.Where(c => !c.Teachers.Any(t => t.ClassName == c.Name)).Select(c => new ClassViewModel
            {
                Id = c.Id,
                Name = c.Name,
            });
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
    }
}
