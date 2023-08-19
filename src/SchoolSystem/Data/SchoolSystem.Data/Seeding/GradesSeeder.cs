namespace SchoolSystem.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;

    public class GradesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            //if (dbContext.Grades.Count() < 50)
            //{
            //    var reasons = new List<GradeReason>
            //    {
            //
            //    GradeReason.Oral, GradeReason.Quiz, GradeReason.Participation, GradeReason.Other,
            //    };
            //    Random rnd = new Random();
            //    for (int i = 0; i < 50; i++)
            //    {
            //        int studentId = 1;
            //        int teacherId = 1;
            //        int subjectId = rnd.Next(1, 12);
            //        double value = rnd.Next(2, 7);
            //        var reason = reasons[rnd.Next(0, reasons.Count)];
            //        var grade = new Grade()
            //        {
            //            SubjectId = subjectId,
            //            StudentId = studentId,
            //            TeacherId = teacherId,
            //            Reason = reason,
            //            Value = value,
            //        };
            //        dbContext.Grades.Add(grade);
            //    }
            //
            //    await dbContext.SaveChangesAsync();
            //}
        }
    }
}
