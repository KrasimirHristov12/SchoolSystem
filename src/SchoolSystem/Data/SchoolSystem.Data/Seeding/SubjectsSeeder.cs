namespace SchoolSystem.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SchoolSystem.Data.Models;

    public class SubjectsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var subjects = new List<string>()
            {
                "Български език и литература", "Математика", "Английски език", "Руски език", "Немски език", "Френски език", "История", "География", "Физика и астрономия",
                "Химия и опазване на околната среда", "Биология и здравно образование",
            };

            if (dbContext.Subjects.Count() == 0)
            {
                foreach (var subject in subjects)
                {
                    await dbContext.Subjects.AddAsync(new Subject
                    {
                        Name = subject,
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
