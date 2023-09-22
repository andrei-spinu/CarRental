using System;
using System.ComponentModel.DataAnnotations;
using CarRental.API.Utils.Validators;

namespace CarRental.API.Models
{
	public class NewReservationDto
	{
        [Required]
        [FutureDate(ErrorMessage = "Start date must be in the future.")]
        public DateTime StartDate { get; set; }
        [Required]
        [DateAfter("StartDate", ErrorMessage = "End date must be after the start date.")]
        public DateTime EndDate { get; set; }

    }
}

