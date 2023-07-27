namespace SchoolSystem.Services.Data.Questions
{
    using System;
    using System.Collections.Generic;

    public interface IQuestionsService
    {
        IEnumerable<Guid> GetIdsByQuizId(Guid quizId);
    }
}
