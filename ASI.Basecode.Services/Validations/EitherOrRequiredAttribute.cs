using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Validations
{
    /// <summary>
    /// Validates that at least one of the specified properties is provided,
    /// and ensures that both properties are not set at the same time.
    /// </summary>
    /// <remarks>
    /// Use this attribute to enforce that it must have either one property 
    /// or the other set, but not both. This is useful in scenarios where 
    /// a choice must be made between two options.
    /// </remarks>
    public class EitherOrRequiredAttribute : ValidationAttribute
    {
        private readonly string _propertyName1;
        private readonly string _propertyName2;

        public string NeitherMessage { get; set; } = "At least one of {0} or {1} must be provided.";
        public string BothMessage { get; set; } = "Only one of {0} or {1} should be provided.";

        public EitherOrRequiredAttribute(string propertyName1, string propertyName2)
        {
            _propertyName1 = propertyName1;
            _propertyName2 = propertyName2;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Retrieve property values
            object value1 = GetPropertyValue(value, _propertyName1);
            object value2 = GetPropertyValue(value, _propertyName2);

            if (IsNullOrEmpty(value1) && IsNullOrEmpty(value2))
            {
                return new ValidationResult(string.Format(NeitherMessage, _propertyName1, _propertyName2));
            }

            if (!IsNullOrEmpty(value1) && !IsNullOrEmpty(value2))
            {
                return new ValidationResult(string.Format(BothMessage, _propertyName1, _propertyName2));
            }

            return ValidationResult.Success;
        }

        private bool IsNullOrEmpty(object value)
        {
            return value == null || (value is string str && string.IsNullOrWhiteSpace(str));
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return null;

            var property = obj.GetType().GetProperty(propertyName);
            return property?.GetValue(obj);
        }
    }
}
