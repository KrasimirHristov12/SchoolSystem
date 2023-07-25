namespace SchoolSystem.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using SchoolSystem.Common;

    public class MaxGreaterThanMin : ValidationAttribute, IClientModelValidator
    {

        public void AddValidation(ClientModelValidationContext context)
        {
            this.MergeAttribute(context.Attributes, "data-val-maxgreaterthanmin", GlobalConstants.ErrorMessage.MaxShouldBeGreaterThanMin);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueAsString = value as string;

            var valueSplitted = valueAsString.Split('-').Select(int.Parse).ToList();
            int minValue = valueSplitted[0];
            int maxValue = valueSplitted[1];

            if (maxValue <= minValue)
            {
                return new ValidationResult(GlobalConstants.ErrorMessage.MaxShouldBeGreaterThanMin);
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
