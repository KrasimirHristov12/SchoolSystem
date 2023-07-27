namespace SchoolSystem.Services.Data.Questions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SchoolSystem.Data;

    public class QuestionsService : IQuestionsService
    {
        private readonly ApplicationDbContext db;

        public QuestionsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Guid> GetIdsByQuizId(Guid quizId)
        {
            return this.db.Questions.Where(q => q.QuizId == quizId).Select(q => q.Id).ToList();
        }
    }
}
