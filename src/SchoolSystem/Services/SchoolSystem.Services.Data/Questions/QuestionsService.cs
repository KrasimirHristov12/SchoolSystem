namespace SchoolSystem.Services.Data.Questions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;

    public class QuestionsService : IQuestionsService
    {
        private readonly IDeletableEntityRepository<Question> questionsRepo;

        public QuestionsService(IDeletableEntityRepository<Question> questionsRepo)
        {
            this.questionsRepo = questionsRepo;
        }

        public IEnumerable<Guid> GetIdsByQuizId(Guid quizId)
        {
            return this.questionsRepo.AllAsNoTracking().Where(q => q.QuizId == quizId).Select(q => q.Id).ToList();
        }
    }
}
