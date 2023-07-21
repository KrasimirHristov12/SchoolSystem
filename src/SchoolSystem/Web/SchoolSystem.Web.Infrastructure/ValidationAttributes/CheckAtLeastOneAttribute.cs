namespace SchoolSystem.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    using SchoolSystem.Common;

    public class CheckAtLeastOneAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.MergeAttribute(context.Attributes, "data-val-checkatleastone", GlobalConstants.ErrorMessage.CheckAtLeastOneAnswer);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = value.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            var boolProperties = properties.Where(p => p.PropertyType == typeof(bool)).Select(p => (bool)p.GetValue(value)).ToList();

            if (!boolProperties.Any(p => p == true))
            {
                return new ValidationResult(GlobalConstants.ErrorMessage.CheckAtLeastOneAnswer);
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
