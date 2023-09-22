using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Utils.Validators
{
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public DateAfterAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDatePropertyInfo = validationContext.ObjectType.GetProperty(_startDatePropertyName);

            if (startDatePropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_startDatePropertyName}");
            }

            var startDateValue = (DateTime)startDatePropertyInfo.GetValue(validationContext.ObjectInstance);
            var endDateValue = (DateTime)value;

            if (endDateValue <= startDateValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}

