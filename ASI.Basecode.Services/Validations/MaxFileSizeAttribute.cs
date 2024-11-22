using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Validations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSizeInMb;

        public MaxFileSizeAttribute(int maxSizeInMb)
        {
            _maxSizeInMb = maxSizeInMb;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > _maxSizeInMb * 1024 * 1024)
                    {
                        return new ValidationResult($"File size cannot exceed {_maxSizeInMb} MB.");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
