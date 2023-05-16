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
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Accounts;

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

        public async Task<CRUDResult> RegisterAsync(RegisterInputModel model)
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
                    return new CRUDResult()
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
                        return new CRUDResult()
                        {
                            Succeeded = false,
                            ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassDoesNotExist },
                        };
                    }

                    if (this.db.Teachers.Any(t => t.ClassName == teacherClassName))
                    {
                        return new CRUDResult()
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

                return new CRUDResult()
                {
                    Succeeded = false,
                    ErrorMessages = errorMessages,
                };
            }

            if ((int)model.TeacherStudent == 0)
            {
                var student = await this.CreateStudentAsync(model, registerUser);

                await this.userManager.AddToRoleAsync(student.User, GlobalConstants.Student.StudentRoleName);
            }
            else if ((int)model.TeacherStudent == 1)
            {
                if (model.IsClassTeacher)
                {
                    var teacher = await this.CreateTeacherAsync(model, registerUser, teacherClassName, (int)model.TeacherClassId);

                    await this.userManager.AddToRoleAsync(teacher.User, GlobalConstants.Teacher.TeacherRoleName);
                }
                else
                {
                    var teacher = await this.CreateTeacherAsync(model, registerUser);

                    await this.userManager.AddToRoleAsync(teacher.User, GlobalConstants.Teacher.TeacherRoleName);
                }
            }

            return new CRUDResult()
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

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        private async Task<Student> CreateStudentAsync(RegisterInputModel model, ApplicationUser user)
        {
            var student = new Student()
            {
                Egn = model.Egn,
                FirstName = model.FirstName,
                Surname = model.Surname,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                ClassId = (int)model.StudentClassId,
                User = user,
            };
            await this.db.Students.AddAsync(student);
            await this.db.SaveChangesAsync();
            return student;
        }

        private async Task<Teacher> CreateTeacherAsync(RegisterInputModel model, ApplicationUser user, string teacherClassName, int teacherClassId)
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
                User = user,
            };
            await this.db.Teachers.AddAsync(teacher);
            var foundClass = await this.db.Classes.FindAsync(teacherClassId);
            teacher.Classes.Add(foundClass);
            await this.db.SaveChangesAsync();
            return teacher;
        }

        private async Task<Teacher> CreateTeacherAsync(RegisterInputModel model, ApplicationUser user)
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
                User = user,
            };
            await this.db.Teachers.AddAsync(teacher);
            await this.db.SaveChangesAsync();
            return teacher;
        }
    }
}
