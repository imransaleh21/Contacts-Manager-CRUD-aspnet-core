using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// This static method is used to get the validation message of any model object
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="ArgumentException">if validation message is found it will be thrown</exception>
        internal static void ValidateTheModelObject(object obj)
        {
            // validate the model object
            ValidationContext validationContext = new(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
