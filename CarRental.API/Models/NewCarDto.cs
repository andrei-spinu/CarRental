using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Models
{
	public class NewCarDto
    { 
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public decimal DailyRate { get; set; }
        [Required]
        public bool Available { get; set; }
        public string? Description { get; set; }

        public NewCarDto(string make, string model, int year, string color, string registrationNumber, decimal dailyRate, bool available)
        {
            Make = make;
            Model = model;
            Year = year;
            Color = color;
            RegistrationNumber = registrationNumber;
            DailyRate = dailyRate;
            Available = available;
        }
    }
}

