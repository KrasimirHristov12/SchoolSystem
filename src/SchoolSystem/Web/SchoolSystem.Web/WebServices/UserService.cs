namespace SchoolSystem.Web.WebServices
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Data.SchoolClass;
    using SchoolSystem.Web.Infrastructure.HubHelpers;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Accounts;
    using SchoolSystem.Web.ViewModels.Chat;

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

        public string GetUserId(ClaimsPrincipal user)
        {
            return user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
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
                    teacherClassName = this.schoolClassService.GetClassNameById((int)model.TeacherClassId);
                    if (teacherClassName == null)
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

        public IEnumerable<UserChatViewModel> GetInfoForOnlineUsers(string excludeUserId)
        {
            var studentsUserInfo = this.db.Students.Where(s => ConnectedUser.Ids.Contains(s.UserId) && s.UserId != excludeUserId)
                .Select(s => new UserChatViewModel
                {
                    FullName = s.FirstName + " " + s.Surname + " " + s.LastName,
                    Username = s.User.UserName,
                }).ToList();

            var teachersUserInfo = this.db.Teachers.Where(t => ConnectedUser.Ids.Contains(t.UserId) && t.UserId != excludeUserId)
                .Select(t => new UserChatViewModel
                {
                    FullName = t.FirstName + " " + t.Surname + " " + t.LastName,
                    Username = t.User.UserName,
                }).ToList();

            return studentsUserInfo.Concat(teachersUserInfo).ToList();
        }

        public async Task<string> GetUsernameByIdAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            return user?.UserName;
        }

        public string GetFullNameById(string userId)
        {
            var studentFullName = this.db.Students.Where(s => s.UserId == userId).Select(s => s.FirstName + " " + s.Surname + " " + s.LastName).FirstOrDefault();
            if (studentFullName == null)
            {
                var teacherFullName = this.db.Teachers.Where(t => t.UserId == userId).Select(t => t.FirstName + " " + t.Surname + " " + t.LastName).FirstOrDefault();

                if (teacherFullName == null)
                {
                    return null;
                }

                return teacherFullName;
            }

            return studentFullName;
        }

        public string GetFullNameByUsername(string username)
        {
            var studentFullName = this.db.Students.Where(s => s.User.UserName == username).Select(s => s.FirstName + " " + s.Surname + " " + s.LastName).FirstOrDefault();
            if (studentFullName == null)
            {
                var teacherFullName = this.db.Teachers.Where(t => t.User.UserName == username).Select(t => t.FirstName + " " + t.Surname + " " + t.LastName).FirstOrDefault();

                if (teacherFullName == null)
                {
                    return null;
                }

                return teacherFullName;
            }

            return studentFullName;
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            return user?.Id;
        }

        public async Task<string> GetEmailByUserIdAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            return user?.Email;
        }

        public async Task<string> GetFullNameByUserIdAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return user.FirstName + " " + user.LastName;
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
