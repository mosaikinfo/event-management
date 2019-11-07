using System.ComponentModel.DataAnnotations;

namespace EventManagement.ApplicationCore.Validation
{
    /// <summary>
    /// Validator to check all rules which have been defined using
    /// the <see cref="System.ComponentModel.DataAnnotations"/> classes.
    /// </summary>
    public class ModelValidator
    {
        /// <summary>
        /// Determines whether the given object is valid.
        /// Throws a <see cref="ValidationException"/> if not.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        public static void Validate(object obj)
        {
            Validator.ValidateObject(obj, new ValidationContext(obj));
        }
    }
}
