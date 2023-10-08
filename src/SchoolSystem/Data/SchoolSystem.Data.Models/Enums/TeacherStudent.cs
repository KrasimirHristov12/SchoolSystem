namespace SchoolSystem.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Common;

    public enum TeacherStudent
    {
        [Display(Name = GlobalConstants.Student.StudentTypeDisplay)]
        Student,
        [Display(Name = GlobalConstants.Teacher.TeacherDisplay)]
        Teacher,
    }
}
