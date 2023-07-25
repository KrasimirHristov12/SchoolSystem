namespace SchoolSystem.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using SchoolSystem.Common;

    public class PoorMinValueZeroAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            this.MergeAttribute(context.Attributes, "data-val-poorminvaluezero", GlobalConstants.ErrorMessage.MinValueShouldBeZero);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueAsString = value as string;
            var minValue = int.Parse(valueAsString.Split('-')[0]);
            if (minValue != 0)
            {
                return new ValidationResult(GlobalConstants.ErrorMessage.MinValueShouldBeZero);
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
