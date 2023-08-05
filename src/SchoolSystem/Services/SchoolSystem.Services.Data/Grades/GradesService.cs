﻿namespace SchoolSystem.Services.Data.Grades
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SchoolSystem.Common;
    using SchoolSystem.Data;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Data.GradingScale;
    using SchoolSystem.Web.ViewModels;
    using SchoolSystem.Web.ViewModels.Grades;

    public class GradesService : IGradesService
    {
        private readonly ApplicationDbContext db;
        private readonly IGradingScaleService gradingScaleService;

        public GradesService(ApplicationDbContext db, IGradingScaleService gradingScaleService)
        {
            this.db = db;
            this.gradingScaleService = gradingScaleService;
        }

        public DisplayGradesViewModel GetForStudent(int studentId, int page)
        {
            var allGradesForStudent = this.db.Grades.Where(g => g.StudentId == studentId).Include(g => g.Teacher).Include(g => g.Subject);
            var model = new DisplayGradesViewModel();
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
                string reasonAsString = this.GetReasonNameByReasonEnum(g.Reason);
                int reasonId = (int)g.Reason;
                string grade = g.Value.ToString("F2");
                int gradeId = g.Id;
                teachersSet.Add(teacherFullName);
                teachersIdsSet.Add(teacherId);
                subjectsSet.Add(subjectName);
                subjectsIdsSet.Add(subjectId);
                reasonsSet.Add(reasonAsString);
                reasonsIdsSet.Add(reasonId);
                gradesSet.Add(grade);
                gradesIdsSet.Add(gradeId);
            }

            var totalGradesForStudent = this.db.Grades.Where(g => g.StudentId == studentId).Count();

            var grades = allGradesForStudent.Skip((page - 1) * 10).Take(10).Select(g => new GradesViewModel
            {
                TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName,
                Date = g.CreatedOn.Date.ToString("d", cultureInfo),
                SubjectName = g.Subject.Name,
                Reason = g.Reason,
                Value = g.Value.ToString("F2"),
            }).ToList();

            foreach (var g in grades)
            {
                g.ReasonAsString = this.GetReasonNameByReasonEnum(g.Reason);
            }

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
            model.CurrentPage = page;
            model.TotalPages = (int)Math.Ceiling(totalGradesForStudent / 10M);
            return model;
        }

        public IEnumerable<GradesViewModel> GetForStudentApi(int studentId, int page)
        {
            var cultureInfo = new CultureInfo("bg-BG");
            return this.db.Grades.Where(g => g.StudentId == studentId).Skip((page - 1) * 10).Take(10).Select(g => new GradesViewModel
            {
                TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName,
                Date = g.CreatedOn.Date.ToString("d", cultureInfo),
                SubjectName = g.Subject.Name,
                Reason = g.Reason,
                Value = g.Value.ToString("F2"),
            }).ToList();
        }

        public IEnumerable<GradesViewModel> GetFilteredGrades(IEnumerable<int> teacherIds, IEnumerable<int> subjectIds, IEnumerable<int> reasonIds, ICollection<int> gradesIds, int date, int studentId)
        {
            var cultureInfo = new CultureInfo("bg-BG");
            var gradesFromDb = this.db.Grades.Where(g => g.StudentId == studentId);
            if (teacherIds.Any())
            {
                gradesFromDb = gradesFromDb.Where(g => teacherIds.Contains(g.TeacherId));
            }

            if (subjectIds.Any())
            {
                gradesFromDb = gradesFromDb.Where(g => subjectIds.Contains(g.SubjectId));
            }

            if (reasonIds.Any())
            {
                gradesFromDb = gradesFromDb.Where(g => reasonIds.Contains((int)g.Reason));
            }

            if (gradesIds.Any())
            {
                gradesFromDb = gradesFromDb.Where(g => gradesIds.Contains(g.Id));
            }
            if (date == 1)
            {
                gradesFromDb = gradesFromDb.OrderByDescending(g => g.CreatedOn);
            }
            else if (date == 2)
            {
                gradesFromDb = gradesFromDb.OrderBy(g => g.CreatedOn);
            }

            var gradesList = gradesFromDb.Select(g => new GradesViewModel
            {
                TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName,
                Date = g.CreatedOn.Date.ToString("d", cultureInfo),
                SubjectName = g.Subject.Name,
                Reason = g.Reason,
                Value = g.Value.ToString("F2"),
            }).ToList();

            foreach (var g in gradesList)
            {
                g.ReasonAsString = this.GetReasonNameByReasonEnum(g.Reason);
            }

            return gradesList;
        }

        public async Task<CRUDResult> AddAsync(GradesInputModel model, int teacherId)
        {
            var selectedStudent = this.db.Students.Include(s => s.Grades).FirstOrDefault(st => st.Id == model.StudentId);
            var selectedClass = this.db.Classes.Include(c => c.Students).FirstOrDefault(c => c.Id == model.ClassId);
            var selectedSubject = this.db.Subjects.FirstOrDefault(s => s.Id == model.SubjectId);
            var teacher = this.db.Teachers.Include(t => t.Subjects).First(t => t.Id == teacherId);

            if (selectedStudent == null)
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.StudentDoesNotExist },
                };
            }

            if (selectedClass == null)
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.ClassDoesNotExist },
                };
            }

            if (selectedSubject == null)
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.SubjectDoesNotExist },
                };
            }

            if (!selectedClass.Students.Any(s => s.Id == selectedStudent.Id))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.StudentNotInClass },
                };
            }

            if (!teacher.Subjects.Any(s => s.Id == model.SubjectId))
            {
                return new CRUDResult
                {
                    Succeeded = false,
                    ErrorMessages = new List<string> { GlobalConstants.ErrorMessage.SubjectNotInTeacherList },
                };
            }

            selectedStudent.Grades.Add(new Grade
            {
                TeacherId = teacherId,
                SubjectId = (int)model.SubjectId,
                Reason = (GradeReason)model.Reason,
                Value = (double)model.Value,
            });
            await this.db.SaveChangesAsync();

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

        public async Task<bool> AddAfterQuizIsTakenAsync(int teacherId, int studentId, int subjectId, int pointsEarned, IEnumerable<string> scaleRanges)
        {
            if (!this.db.Teachers.Any(t => t.Id == teacherId))
            {
                return false;
            }

            if (!this.db.Students.Any(s => s.Id == studentId))
            {
                return false;
            }

            if (!this.db.Subjects.Any(s => s.Id == subjectId))
            {
                return false;
            }

            var markNumber = this.GetMarkByPointsEarnedAndGradingScale(pointsEarned, scaleRanges);
            var markObj = new Grade
            {
                TeacherId = teacherId,
                StudentId = studentId,
                SubjectId = subjectId,
                Reason = GradeReason.Quiz,
                Value = markNumber,
            };

            this.db.Grades.Add(markObj);

            await this.db.SaveChangesAsync();
            return true;
        }

        public string GetReasonNameByReasonEnum(GradeReason grade)
        {
            var enumDisplayDict = new Dictionary<GradeReason, string>();

            enumDisplayDict[GradeReason.Oral] = GlobalConstants.Grade.OralReason;
            enumDisplayDict[GradeReason.Quiz] = GlobalConstants.Grade.QuizReason;
            enumDisplayDict[GradeReason.Participation] = GlobalConstants.Grade.ParticipationReason;
            enumDisplayDict[GradeReason.Other] = GlobalConstants.Grade.OtherReason;
            return enumDisplayDict[grade];
        }
    }
}
