namespace SchoolSystem.Web.ViewModels.Quizzes
{
    using System;

    public class DisplayQuizzesViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SubjectName { get; set; }

        public string TeacherName { get; set; }

        public int Duration { get; set; }

        public DateTime DateTaken { get; set; }

        public bool? IsTaken { get; set; }

        public int? Points { get; set; }

    }
}
