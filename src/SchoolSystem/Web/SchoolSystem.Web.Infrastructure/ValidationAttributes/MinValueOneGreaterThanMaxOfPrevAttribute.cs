namespace SchoolSystem.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using SchoolSystem.Common;

    public class MinValueOneGreaterThanMaxOfPrevAttribute : ValidationAttribute, IClientModelValidator
    {
        public MinValueOneGreaterThanMaxOfPrevAttribute(string otherProperty)
        {
            this.OtherProperty = otherProperty;
        }

        public string OtherProperty { get; set; }

        public void AddValidation(ClientModelValidationContext context)
        {
            this.MergeAttribute(context.Attributes, "data-val-minvalueonegreaterthanmaxofprev", GlobalConstants.ErrorMessage.MinValueShouldBeOneGreaterThanMaxValueOfPrev);
            this.MergeAttribute(context.Attributes, "data-val-minvalueonegreaterthanmaxofprev-otherproperty", this.OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueAsString = value as string;
            var otherPropertyValueAsString = validationContext.ObjectInstance.GetType().GetProperty(this.OtherProperty).GetValue(validationContext.ObjectInstance, null) as string;

            var valueSplitted = valueAsString.Split('-').Select(int.Parse).ToList();

            var otherPropertyValueSplitted = otherPropertyValueAsString.Split("-").Select(int.Parse).ToList();

            int minValue = valueSplitted[0];

            int maxValueOtherProperty = otherPropertyValueSplitted[1];

            if (minValue != maxValueOtherProperty + 1)
            {
                return new ValidationResult(string.Format(GlobalConstants.ErrorMessage.MinValueShouldBeOneGreaterThanMaxValueOfPrev, maxValueOtherProperty + 1, minValue));
            }

            return ValidationResult.Success;

        }

        private bool MergeAttribute(
IDictionary<string, string> attributes,
string key,
string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}
