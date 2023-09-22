using System;
using CarRental.API.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.API.Models
{
    public class CarDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public decimal DailyRate { get; set; }
        [Required]
        public bool Available { get; set; }
        public string? Description { get; set; }

    }
}

