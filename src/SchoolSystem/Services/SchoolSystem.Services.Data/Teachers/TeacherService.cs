namespace SchoolSystem.Services.Data.Teachers
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Data;

    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext db;

        public TeacherService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public int GetTeacherIdByUserId(string userId)
        {
            var teacher = this.db.Teachers.FirstOrDefault(t => t.UserId == userId);
            if (teacher == null)
            {
                return -1;
            }

            return teacher.Id;
        }
    }
}
