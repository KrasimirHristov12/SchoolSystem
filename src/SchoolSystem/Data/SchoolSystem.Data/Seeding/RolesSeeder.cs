namespace SchoolSystem.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Models;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // while (userManager.Users.Count() > 0)
            // {
            //     var users = userManager.Users.ToList();
            //     await userManager.DeleteAsync(users[users.Count - 1]);
            // }
            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.Teacher.TeacherRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.Student.StudentRoleName);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
