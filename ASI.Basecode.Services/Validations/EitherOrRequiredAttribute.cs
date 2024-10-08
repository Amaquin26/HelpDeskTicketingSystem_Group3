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
    /// Use this attribute to enforce that a ViewModel must have either one property 
    /// or the other set, but not both. This is useful in scenarios where 
    /// a choice must be made between two options.
    /// </remarks>
    public class EitherOrRequiredAttribute: ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public string NeitherMessage { get; set; } = "At least one of {0} or {1} must be provided.";
        public string BothMessage { get; set; } = "Only one of {0} or {1} should be provided.";

        public EitherOrRequiredAttribute(string propertyName1, string propertyName2)
        {
            _propertyNames = new[] { propertyName1, propertyName2 };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property1 = validationContext.ObjectType.GetProperty(_propertyNames[0]);
            var property2 = validationContext.ObjectType.GetProperty(_propertyNames[1]);

            if (property1 == null || property2 == null)
            {
                throw new ArgumentException("Both properties must be defined.");
            }

            var value1 = property1.GetValue(validationContext.ObjectInstance);
            var value2 = property2.GetValue(validationContext.ObjectInstance);

            if (value1 == null && value2 == null)
            {
                return new ValidationResult(string.Format(NeitherMessage, _propertyNames[0], _propertyNames[1]));
            }

            if (value1 != null && value2 != null)
            {
                return new ValidationResult(string.Format(BothMessage, _propertyNames[0], _propertyNames[1]));
            }

            return ValidationResult.Success;
        }
    }
}
