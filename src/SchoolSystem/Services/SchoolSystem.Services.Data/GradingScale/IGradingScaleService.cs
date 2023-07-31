namespace SchoolSystem.Services.Data.GradingScale
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SchoolSystem.Web.ViewModels.GradingScale;

    public interface IGradingScaleService
    {
        Task<bool> AddAsync(Guid quizId, string gradingScaleForPoor, string gradingScaleForFair, string gradingScaleForGood, string gradingScaleForVeryGood, string gradingScaleForExcellent);

        List<int> GetMinMaxPoints(string gradingScale);

        GradingScaleViewModel GetGradingScale(Guid quizId);
    }
}
