namespace SchoolSystem.Web.ViewModels.Grades
{
    using SchoolSystem.Data.Models.Enums;
    using System;

    public class GradesViewModel
    {
        public string TeacherName { get; set; }

        public string SubjectName { get; set; }

        public string Date { get; set; }
        public GradeReason Reason { get; set; }

        public string ReasonAsString { get; set; }

        public string Value { get; set; }
    }
}
