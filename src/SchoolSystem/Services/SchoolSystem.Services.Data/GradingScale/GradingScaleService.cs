namespace SchoolSystem.Services.Data.GradingScale
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SchoolSystem.Data.Common.Repositories;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Web.ViewModels.GradingScale;

    public class GradingScaleService : IGradingScaleService
    {
        private readonly IDeletableEntityRepository<Quiz> quizzesRepo;
        private readonly IDeletableEntityRepository<GradingScale> scalesRepo;

        public GradingScaleService(IDeletableEntityRepository<Quiz> quizzesRepo, IDeletableEntityRepository<GradingScale> scalesRepo)
        {
            this.quizzesRepo = quizzesRepo;
            this.scalesRepo = scalesRepo;
        }

        public async Task<bool> AddAsync(Guid quizId, string gradingScaleForPoor, string gradingScaleForFair, string gradingScaleForGood, string gradingScaleForVeryGood, string gradingScaleForExcellent)
        {
            if (!this.quizzesRepo.AllAsNoTracking().Any(q => q.Id == quizId))
            {
                return false;
            }

            var gradingScaleForPoorAsList = this.GetMinMaxPoints(gradingScaleForPoor);
            var gradingScaleForFairAsList = this.GetMinMaxPoints(gradingScaleForFair);
            var gradingScaleForGoodAsList = this.GetMinMaxPoints(gradingScaleForGood);
            var gradingScaleForVeryGoodAsList = this.GetMinMaxPoints(gradingScaleForVeryGood);
            var gradingScaleForExcellentAsList = this.GetMinMaxPoints(gradingScaleForExcellent);

            int minPointsPoor = gradingScaleForPoorAsList[0];

            int maxPointsPoor = gradingScaleForPoorAsList[1];

            int minPointsFair = gradingScaleForFairAsList[0];

            int maxPointsFair = gradingScaleForFairAsList[1];

            int minPointsGood = gradingScaleForGoodAsList[0];

            int maxPointsGood = gradingScaleForGoodAsList[1];

            int minPointsForVeryGood = gradingScaleForVeryGoodAsList[0];

            int maxPointsForVeryGood = gradingScaleForVeryGoodAsList[1];

            int minPointsForExcellent = gradingScaleForExcellentAsList[0];

            int maxPointsForExcellent = gradingScaleForExcellentAsList[1];

            var gradingScale = new GradingScale
            {
                QuizId = quizId,
                MinimumPointForPoor = minPointsPoor,
                MaximumPointsForPoor = maxPointsPoor,
                MinimumPointsForFair = minPointsFair,
                MaximumPointsForFair = maxPointsFair,
                MinimumPointsForGood = minPointsGood,
                MaximumPointsForGood = maxPointsGood,
                MinimumPointsForVeryGood = minPointsForVeryGood,
                MaximumPointsForVeryGood = maxPointsForVeryGood,
                MinimumPointsForExcellent = minPointsForExcellent,
                MaximumPointsForExcellent = maxPointsForExcellent,
            };
            await this.scalesRepo.AddAsync(gradingScale);
            await this.scalesRepo.SaveChangesAsync();
            return true;
        }

        public List<int> GetMinMaxPoints(string gradingScale)
        {
            return gradingScale.Split("-").Select(int.Parse).ToList();
        }

        public GradingScaleViewModel GetGradingScale(Guid quizId)
        {
            var gradingScale = this.scalesRepo.AllAsNoTracking().Where(g => g.QuizId == quizId).Select(g => new GradingScaleViewModel
            {
                ScaleRangeForPoor = $"{g.MinimumPointForPoor}-{g.MaximumPointsForPoor}",
                ScaleRangeForFair = $"{g.MinimumPointsForFair}-{g.MaximumPointsForFair}",
                ScaleRangeForGood = $"{g.MinimumPointsForGood}-{g.MaximumPointsForGood}",
                ScaleRangeForVeryGood = $"{g.MinimumPointsForVeryGood}-{g.MaximumPointsForVeryGood}",
                ScaleRangeForExcellent = $"{g.MinimumPointsForExcellent}-{g.MaximumPointsForExcellent}",
            }).FirstOrDefault();

            return gradingScale;
        }
    }
}
