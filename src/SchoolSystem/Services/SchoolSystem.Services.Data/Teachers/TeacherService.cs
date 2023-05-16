namespace SchoolSystem.Services.Data.Teachers
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Data;

    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext db;

        public TeacherService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<int> GetTeacherIdByUserIdAsync(string userId)
        {
            var teacher = await this.db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null)
            {
                return -1;
            }

            return teacher.Id;
        }
    }
}
