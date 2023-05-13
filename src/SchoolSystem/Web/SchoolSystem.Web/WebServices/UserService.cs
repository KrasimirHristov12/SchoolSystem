namespace SchoolSystem.Web.WebServices
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Web.ViewModels.Accounts;
    using SchoolSystem.Web.WebModels;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext db;
        private readonly ISchoolClassService schoolClassService;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, ISchoolClassService schoolClassService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
            this.schoolClassService = schoolClassService;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterInputModel model)
        {
            string teacherClassName = null;

            var registerUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            if ((int)model.TeacherStudent == 0)
            {
                if (!this.db.Classes.Any(c => c.Id == model.StudentClassId))
                {
                    return new RegisterResult()
                    {
                        Succeeded = false,
                        ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassDoesNotExist },
                    };
                }
            }
            else if ((int)model.TeacherStudent == 1)
            {
                if (model.IsClassTeacher)
                {
                    teacherClassName = await this.schoolClassService.GetClassNameById((int)model.TeacherClassId);
                    if (teacherClassName == string.Empty)
                    {
                        return new RegisterResult()
                        {
                            Succeeded = false,
                            ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassDoesNotExist },
                        };
                    }

                    if (this.db.Teachers.Any(t => t.ClassName == teacherClassName))
                    {
                        return new RegisterResult()
                        {
                            Succeeded = false,
                            ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.TeacherAlreadyHeadForThisClass },
                        };
                    }
                }
            }

            var result = await this.userManager.CreateAsync(registerUser, model.Password);

            if (!result.Succeeded)
            {
                var errorMessages = new List<string>();

                foreach (var e in result.Errors)
                {
                    if ((e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName") && !errorMessages.Contains(GlobalConstants.ErrorMessage.EmailAlreadyTaken))
                    {
                        errorMessages.Add(GlobalConstants.ErrorMessage.EmailAlreadyTaken);
                    }
                }

                return new RegisterResult()
                {
                    Succeeded = false,
                    ErrorMessages = errorMessages,
                };
            }

            if ((int)model.TeacherStudent == 0)
            {
                await this.CreateStudentAsync(model);
            }
            else if ((int)model.TeacherStudent == 1)
            {
                if (model.IsClassTeacher)
                {
                    await this.CreateTeacherAsync(model, teacherClassName, (int)model.TeacherClassId);
                }
                else
                {
                    await this.CreateTeacherAsync(model);
                }
            }

            return new RegisterResult()
            {
                Succeeded = true,
                ErrorMessages = null,
            };
        }

        public async Task<bool> LoginAsync(LoginInputModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return false;
            }

            var result = await this.signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        private async Task CreateStudentAsync(RegisterInputModel model)
        {
            var student = new Student()
            {
                Egn = model.Egn,
                FirstName = model.FirstName,
                Surname = model.Surname,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                ClassId = (int)model.StudentClassId,
            };
            await this.db.Students.AddAsync(student);
            await this.db.SaveChangesAsync();
        }

        private async Task CreateTeacherAsync(RegisterInputModel model, string teacherClassName, int teacherClassId)
        {
            var teacher = new Teacher()
            {
                Egn = model.Egn,
                FirstName = model.FirstName,
                Surname = model.Surname,
                LastName = model.LastName,
                YearsOfExperience = (int)model.TeacherYearsOfExperience,
                IsClassTeacher = model.IsClassTeacher,
                ClassName = teacherClassName,
                PhoneNumber = model.PhoneNumber,
            };
            await this.db.Teachers.AddAsync(teacher);
            var foundClass = await this.db.Classes.FindAsync(teacherClassId);
            teacher.Classes.Add(foundClass);
            await this.db.SaveChangesAsync();
        }

        private async Task CreateTeacherAsync(RegisterInputModel model)
        {
            var teacher = new Teacher()
            {
                Egn = model.Egn,
                FirstName = model.FirstName,
                Surname = model.Surname,
                LastName = model.LastName,
                YearsOfExperience = (int)model.TeacherYearsOfExperience,
                IsClassTeacher = false,
                PhoneNumber = model.PhoneNumber,
            };
            await this.db.Teachers.AddAsync(teacher);
            await this.db.SaveChangesAsync();
        }
    }
}
