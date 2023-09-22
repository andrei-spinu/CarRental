using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Utils.Validators
{
	public class FutureDateAttribute : ValidationAttribute
	{
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime) value;
            return date > DateTime.Now;
        }
    }
}

