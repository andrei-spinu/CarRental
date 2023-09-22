using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Models
{
	public class CarForUpdateDto
	{
        [Required]
        public string Color { get; set; }
        [Required]
        public decimal DailyRate { get; set; }
        [Required]
        public bool Available { get; set; }
        public string? Description { get; set; }
    }
}

