namespace SchoolSystem.Web.Infrastructure.ValidationAttributes
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using SchoolSystem.Common;

    public class MaximumEqualToTotalPointsAttribute : ValidationAttribute
    {
        public MaximumEqualToTotalPointsAttribute(string questionsPropertyName, string pointsPropertyName)
        {
            this.QuestionsPropertyName = questionsPropertyName;
            this.PointsPropertyName = pointsPropertyName;
        }

        public string QuestionsPropertyName { get; set; }

        public string PointsPropertyName { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int totalPoints = 0;
            var valueAsString = value as string;

            var valueSplitted = valueAsString.Split('-').Select(int.Parse).ToList();

            int maxValue = valueSplitted[1];

            var questionsList = validationContext.ObjectInstance.GetType().GetProperty(this.QuestionsPropertyName).GetValue(validationContext.ObjectInstance, null) as IEnumerable;

            foreach (var question in questionsList)
            {
                var points = (int)question.GetType().GetProperty(this.PointsPropertyName).GetValue(question, null);
                totalPoints += points;
            }

            if (maxValue != totalPoints)
            {
                return new ValidationResult(GlobalConstants.ErrorMessage.MaxValueOfLastShouldBeEqualToTotalPoints);
            }

            return ValidationResult.Success;

        }
    }
}
