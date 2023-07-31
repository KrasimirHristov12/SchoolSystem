namespace SchoolSystem.Services.Data.GradingScale
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SchoolSystem.Data;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Web.ViewModels.GradingScale;

    public class GradingScaleService : IGradingScaleService
    {
        private readonly ApplicationDbContext db;

        public GradingScaleService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddAsync(Guid quizId, string gradingScaleForPoor, string gradingScaleForFair, string gradingScaleForGood, string gradingScaleForVeryGood, string gradingScaleForExcellent)
        {
            var quiz = await this.db.Quizzes.FindAsync(quizId);
            if (quiz == null)
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
            this.db.GradingScales.Add(gradingScale);
            await this.db.SaveChangesAsync();
            return true;
        }

        public List<int> GetMinMaxPoints(string gradingScale)
        {
            return gradingScale.Split("-").Select(int.Parse).ToList();
        }

        public GradingScaleViewModel GetGradingScale(Guid quizId)
        {
            var gradingScale = this.db.GradingScales.Where(g => g.QuizId == quizId).FirstOrDefault();

            if (gradingScale == null)
            {
                return null;
            }

            string gradingScaleForPoor = $"{gradingScale.MinimumPointForPoor}-{gradingScale.MaximumPointsForPoor}";
            string gradingScaleForFair = $"{gradingScale.MinimumPointsForFair}-{gradingScale.MaximumPointsForFair}";
            string gradingScaleForGood = $"{gradingScale.MinimumPointsForGood}-{gradingScale.MaximumPointsForGood}";
            string gradingScaleForVeryGood = $"{gradingScale.MinimumPointsForVeryGood}-{gradingScale.MaximumPointsForVeryGood}";
            string gradingScaleForExcellent = $"{gradingScale.MinimumPointsForExcellent}-{gradingScale.MaximumPointsForExcellent}";

            var gradingScaleViewModel = new GradingScaleViewModel
            {
                ScaleRangeForPoor = gradingScaleForPoor,
                ScaleRangeForFair = gradingScaleForFair,
                ScaleRangeForGood = gradingScaleForGood,
                ScaleRangeForVeryGood = gradingScaleForVeryGood,
                ScaleRangeForExcellent = gradingScaleForExcellent,
            };

            return gradingScaleViewModel;

        }
    }
}
