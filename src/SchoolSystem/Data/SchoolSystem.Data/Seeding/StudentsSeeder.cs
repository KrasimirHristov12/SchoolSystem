namespace SchoolSystem.Data.Seeding
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;

    public class StudentsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Students.Count() < 100)
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                Random rnd = new Random();
                var studentNames = new List<string>
            {
                "Plamen", "Ivan", "Vasil", "Stefan", "Georgi", "Nikola", "Haralambi", "Apostol", "Dimitar", "Hristo", "Emil", "Petar", "Alexander", "Martin", "Viktor", "Vladimir", "Dragomir", "Chavdar", "Stanko", "Bozhidar", "Bojidar", "Desislav", "Momchil", "Atanas", "Doncho", "Andrey", "Tsvetan", "Krastio", "Blagun", "Radoslav", "Grozdan", "Krasimir", "Lyuben", "Ivaylo", "Radimir",
            };

                var studentSurnamesLastNames = new List<string>
            {
                "Ivanov", "Dimitrov", "Vasilev", "Hristov", "Asenov", "Todorov", "Dinkov", "Georgiev", "Tsonev",
            };

                var reasons = new List<GradeReason>
            {
                GradeReason.Oral, GradeReason.Quiz, GradeReason.Participation, GradeReason.Other,
            };

                for (int i = 0; i < 100; i++)
                {
                    string firstName = studentNames[rnd.Next(studentNames.Count)];
                    string surname = studentSurnamesLastNames[rnd.Next(studentSurnamesLastNames.Count)];
                    string lastName = studentSurnamesLastNames[rnd.Next(studentSurnamesLastNames.Count)];
                    var studentUser = new ApplicationUser
                    {
                        UserName = $"{firstName}{lastName}{i + 1}@abv.bg",
                        Email = $"{firstName}{lastName}{i + 1}@abv.bg",
                        PhoneNumber = configuration["Dummy:PhoneNumber"],
                    };
                    var creationResult = await userManager.CreateAsync(studentUser, configuration["Dummy:Password"]);
                    if (creationResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(studentUser, GlobalConstants.Student.StudentRoleName);

                        var student = new Student
                        {
                            FirstName = firstName,
                            Surname = surname,
                            LastName = lastName,
                            ClassId = rnd.Next(1, 17),
                            Egn = configuration["Dummy:Egn"],
                            PhoneNumber = configuration["Dummy:PhoneNumber"],
                            UserId = studentUser.Id,
                        };
                        for (int j = 0; j < 20; j++)
                        {
                            student.Grades.Add(new Grade
                            {
                                SubjectId = rnd.Next(1, 12),
                                StudentId = student.Id,
                                Reason = reasons[rnd.Next(reasons.Count)],
                                Value = rnd.Next(2, 7),
                                TeacherId = 2,
                            });
                        }

                        dbContext.Students.Add(student);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
