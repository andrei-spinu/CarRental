using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Models
{
	public class NewReservationDto
	{
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

    }
}

