namespace SchoolSystem.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using SchoolSystem.Data.Models;

    public class ClassesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var classesToSeed = new[] { "8А", "8Б", "8В", "8Г", "9А", "9Б", "9В", "9Г", "10А", "10Б", "10В", "10Г", "11А", "11Б", "11В", "11Г" };

            if (!dbContext.Classes.Any())
            {
                foreach (var c in classesToSeed)
                {
                    await dbContext.Classes.AddAsync(new SchoolClass
                    {
                        Name = c,
                    });
                }
            }
        }
    }
}
