namespace SchoolSystem.Services.Data.Grades
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Data.GradingScale;
    using SchoolSystem.Services.Data.Notifications;
    using SchoolSystem.Services.Data.Students;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.Infrastructure.ExtensionMethods;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Grades;
    using SchoolSystem.Web.ViewModels.Quizzes;

    public class GradesService : IGradesService
    {
        private readonly IDeletableEntityRepository<Grade> gradesRepo;
        private readonly IDeletableEntityRepository<Student> studentsRepo;
        private readonly IDeletableEntityRepository<SchoolClass> classesRepo;
        private readonly IDeletableEntityRepository<Teacher> teachersRepo;
        private readonly IDeletableEntityRepository<Subject> subjectsRepo;
        private readonly IGradingScaleService gradingScaleService;
        private readonly ITeacherService teacherService;
        private readonly INotificationsService notificationsService;
        private readonly IStudentService studentService;

        public GradesService(IDeletableEntityRepository<Grade> gradesRepo, IDeletableEntityRepository<Student> studentsRepo, IDeletableEntityRepository<SchoolClass> classesRepo, IDeletableEntityRepository<Teacher> teachersRepo, IDeletableEntityRepository<Subject> subjectsRepo, IGradingScaleService gradingScaleService, ITeacherService teacherService, INotificationsService notificationsService, IStudentService studentService)
        {
            this.gradesRepo = gradesRepo;
            this.studentsRepo = studentsRepo;
            this.classesRepo = classesRepo;
            this.teachersRepo = teachersRepo;
            this.subjectsRepo = subjectsRepo;
            this.gradingScaleService = gradingScaleService;
            this.teacherService = teacherService;
            this.notificationsService = notificationsService;
            this.studentService = studentService;
        }

        public DisplayGradesViewModel<T> GetForStudent<T>(int studentId, int page)
        {
            var allGradesForStudent = this.gradesRepo.AllAsNoTracking().Where(g => g.StudentId == studentId).Include(g => g.Teacher).Include(g => g.Subject);
            var model = new DisplayGradesViewModel<T>();
            var cultureInfo = new CultureInfo("bg-BG");
            var teachersSet = new HashSet<string>();
            var subjectsSet = new HashSet<string>();
            var reasonsSet = new HashSet<string>();
            var gradesSet = new HashSet<string>();
            var datesSet = new HashSet<string>
            {
                "Нисходящ", "Възходящ",
            };

            var teachersIdsSet = new HashSet<int>();
            var subjectsIdsSet = new HashSet<int>();
            var reasonsIdsSet = new HashSet<int>();
            var gradesIdsSet = new HashSet<int>();
            var datesIdsSet = new HashSet<int>()
            {
                1, 2,
            };

            foreach (var g in allGradesForStudent)
            {
                string teacherFullName = g.Teacher.FirstName + " " + g.Teacher.LastName;
                int teacherId = g.Teacher.Id;
                string subjectName = g.Subject.Name;
                int subjectId = g.Subject.Id;
                int reasonId = (int)g.Reason;
                string grade = g.Value.ToString("F2");
                int gradeId = g.Id;
                teachersSet.Add(teacherFullName);
                teachersIdsSet.Add(teacherId);
                subjectsSet.Add(subjectName);
                subjectsIdsSet.Add(subjectId);
                reasonsSet.Add(g.Reason.GetDisplayName());
                reasonsIdsSet.Add(reasonId);
                gradesSet.Add(grade);
                gradesIdsSet.Add((int)g.Value);
            }

            var totalGradesForStudent = this.gradesRepo.AllAsNoTracking().Where(g => g.StudentId == studentId).Count();

            var grades = allGradesForStudent.Skip((page - 1) * 10).Take(10).To<T>().ToList();

            model.Filter = new FilterGradesViewModel();
            model.Filter.Teachers = new TeachersFilterViewModel
            {
                EntityIds = teachersIdsSet,
                EntityNames = teachersSet,
            };
            model.Filter.Subjects = new SubjectsFilterViewModel
            {
                EntityIds = subjectsIdsSet,
                EntityNames = subjectsSet,
            };
            model.Filter.Reasons = new ReasonsFilterViewModel
            {
                EntityIds = reasonsIdsSet,
                EntityNames = reasonsSet,
            };
            model.Filter.Grades = new GradesFilterViewModel
            {
                EntityIds = gradesIdsSet,
                EntityNames = gradesSet,
            };
            model.Filter.Dates = new DatesFilterViewModel
            {
                EntityIds = datesIdsSet,
                EntityNames = datesSet,
            };
            model.Grades = grades;
            model.TotalGrades = totalGradesForStudent;
            model.CurrentPage = page;
            model.TotalPages = (int)Math.Ceiling(totalGradesForStudent / 10M);
            return model;
        }

        public DisplayGradesViewModel<T> GetFilteredGrades<T>(int page, int studentId, IEnumerable<int> teacherIds = null, IEnumerable<int> subjectIds = null, IEnumerable<int> reasonIds = null, ICollection<int> gradesValues = null, int? date = null)
        {
            var cultureInfo = new CultureInfo("bg-BG");
            var gradesFromDb = this.gradesRepo.AllAsNoTracking().Where(g => g.StudentId == studentId);
            if (teacherIds != null && teacherIds.Any())
            {
                gradesFromDb = gradesFromDb.Where(g => teacherIds.Contains(g.TeacherId));
            }

            if (subjectIds != null && subjectIds.Any())
            {
                gradesFromDb = gradesFromDb.Where(g => subjectIds.Contains(g.SubjectId));
            }

            if (reasonIds != null && reasonIds.Any())
            {
                gradesFromDb = gradesFromDb.Where(g => reasonIds.Contains((int)g.Reason));
            }

            if (gradesValues != null && gradesValues.Any())
            {
                gradesFromDb = gradesFromDb.Where(g => gradesValues.Contains((int)g.Value));
            }

            if (date != null && date == 1)
            {
                gradesFromDb = gradesFromDb.OrderByDescending(g => g.CreatedOn);
            }
            else if (date != null && date == 2)
            {
                gradesFromDb = gradesFromDb.OrderBy(g => g.CreatedOn);
            }

            var filteredCount = gradesFromDb.Count();

            var gradesList = gradesFromDb.Skip((page - 1) * 10)
                .Take(10).To<T>()
                .ToList();

            return new DisplayGradesViewModel<T>
            {
                Grades = gradesList,
                TotalGrades = filteredCount,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(filteredCount / 10M),
                Filter = null,
            };
        }

        public async Task<CRUDResult> AddAsync(GradesInputModel model, int teacherId, string userId)
        {
            var selectedStudent = this.studentsRepo.All().Where(s => s.Id == model.StudentId).FirstOrDefault();

            if (selectedStudent == null)
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.StudentDoesNotExist },
                };
            }

            if (!this.classesRepo.AllAsNoTracking().Any(c => c.Id == model.ClassId))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassDoesNotExist },
                };
            }

            if (!this.subjectsRepo.AllAsNoTracking().Any(s => s.Id == model.SubjectId))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.SubjectDoesNotExist },
                };
            }

            if (!this.classesRepo.AllAsNoTracking().Any(c => c.Id == model.ClassId && c.Students.Any(s => s.Id == model.StudentId)))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.StudentNotInClass },
                };
            }

            if (!this.subjectsRepo.AllAsNoTracking().Any(s => s.Id == model.SubjectId && s.Teachers.Any(t => t.Id == teacherId)))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.SubjectNotInTeacherList },
                };
            }

            if (!this.teachersRepo.AllAsNoTracking().Any(t => t.Id == teacherId && t.Classes.Any(c => c.Id == model.ClassId)))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.TeacherNotInClass },
                };
            }

            selectedStudent.Grades.Add(new Grade
            {
                TeacherId = teacherId,
                SubjectId = (int)model.SubjectId,
                Reason = (GradeReason)model.Reason,
                Value = (double)model.Value,
            });

            await this.gradesRepo.SaveChangesAsync();

            var studentUserId = this.studentService.GetUserId((int)model.StudentId);
            var receiversIds = new List<string>
            {
                studentUserId,
            };
            string teacherFullName = this.teacherService.GetTeacherFullName(teacherId);
            string notificationMessage = string.Format(GlobalConstants.Notification.AddedGrade, teacherFullName);
            await this.notificationsService.AddAsync(NotificationType.AddedGrade, receiversIds, notificationMessage);

            return new CRUDResult { Succeeded = true, ErrorMessages = null, };
        }

        public int GetMarkByPointsEarnedAndGradingScale(int pointsEarned, IEnumerable<string> scaleRanges)
        {
            int currentMark = 2;
            int finalMark = 0;

            foreach (var scale in scaleRanges)
            {
                var minMaxValues = this.gradingScaleService.GetMinMaxPoints(scale);
                int minValue = minMaxValues[0];
                int maxValue = minMaxValues[1];
                if (Enumerable.Range(minValue, maxValue - minValue + 1).Contains(pointsEarned))
                {
                    finalMark = currentMark;
                    break;
                }

                currentMark++;
            }

            return finalMark;
        }

        public async Task<bool> AddAfterQuizIsTakenAsync(TakeQuizViewModel model, int pointsEarned, IEnumerable<string> scaleRanges)
        {
            if (!this.teachersRepo.AllAsNoTracking().Any(t => t.Id == model.TeacherId))
            {
                return false;
            }

            if (!this.studentsRepo.AllAsNoTracking().Any(s => s.Id == model.StudentId))
            {
                return false;
            }

            if (!this.subjectsRepo.AllAsNoTracking().Any(s => s.Id == model.SubjectId))
            {
                return false;
            }

            var markNumber = this.GetMarkByPointsEarnedAndGradingScale(pointsEarned, scaleRanges);
            var markObj = new Grade
            {
                TeacherId = model.TeacherId,
                StudentId = model.StudentId,
                SubjectId = model.SubjectId,
                Reason = GradeReason.Quiz,
                Value = markNumber,
            };

            await this.gradesRepo.AddAsync(markObj);

            await this.gradesRepo.SaveChangesAsync();

            var takenNotificationMessage = string.Format(GlobalConstants.Notification.TestTaken, model.StudentFullName, model.StudentClassName, model.Name);
            await this.notificationsService.AddAsync(NotificationType.TestTaken, new List<string> { model.TeacherUserId }, takenNotificationMessage);

            var addedNotificationMessage = string.Format(GlobalConstants.Notification.AddedGrade, model.TeacherFullName);
            await this.notificationsService.AddAsync(NotificationType.AddedGrade, new List<string> { model.StudentUserId }, addedNotificationMessage);

            return true;
        }
    }
}
