namespace SchoolSystem.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Models;

    public class AccountsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var headMasterUser = new ApplicationUser
            {
                Email = configuration["DirectorCredentials:EmailAddress"],
                UserName = configuration["DirectorCredentials:EmailAddress"],
                PhoneNumber = configuration["DirectorCredentials:PhoneNumber"],

            };
            var result = await userManager.CreateAsync(headMasterUser, configuration["DirectorCredentials:Password"]);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(headMasterUser, GlobalConstants.Headmaster.HeadmasterRoleName);
            }
            
        }
    }
}
