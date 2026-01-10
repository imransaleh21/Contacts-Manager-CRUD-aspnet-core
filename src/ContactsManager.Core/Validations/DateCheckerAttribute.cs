using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Validations
{
    public class DateCheckerAttribute : ValidationAttribute
    {
        /// <summary>
        /// This attribute is used to validate that the date of birth is at least 18 years ago.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns>validationResult result with message</returns>

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value != null && value is DateTime)
            {
                DateTime minDate = DateTime.Now.AddYears(-18);
                DateTime givenDate = (DateTime)value;
                if (givenDate>minDate)
                {
                    return new ValidationResult("You must be at least 18 years old to register.");
                }
                return ValidationResult.Success;
            }
            return null;
        }
    }
}
