namespace SchoolSystem.Web
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Common;
    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Repositories;
    using SchoolSystem.Data.Seeding;
    using SchoolSystem.Services.Data;
    using SchoolSystem.Services.Data.Chat;
    using SchoolSystem.Services.Data.Contact;
    using SchoolSystem.Services.Data.Grades;
    using SchoolSystem.Services.Data.GradingScale;
    using SchoolSystem.Services.Data.Notifications;
    using SchoolSystem.Services.Data.Questions;
    using SchoolSystem.Services.Data.Quizzes;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Services.Data.Subjects;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Services.Email;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Services.Messaging;
    using SchoolSystem.Web.Hubs;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.WebServices;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = GlobalConstants.PasswordMinLength;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            });

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Accounts/Login";
            });

            services.AddControllersWithViews(
                options =>
                {
                    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISchoolClassService, SchoolClassService>();
            services.AddTransient<ITeacherService, TeacherService>();
            services.AddTransient<ISubjectService, SubjectService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IGradesService, GradesService>();
            services.AddTransient<IQuizzesService, QuizzesService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IGradingScaleService, GradingScaleService>();
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<SchoolSystem.Services.Email.IEmailSender, SchoolSystem.Services.Email.EmailSender>();
            services.AddSingleton<IUserIdProvider, UserIdProviderCustom>();
            services.AddSignalR();
        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<NotificationsHub>("/notificationsHub");
            app.MapHub<ChatHub>("/chatHub");
            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
        }
    }
}
