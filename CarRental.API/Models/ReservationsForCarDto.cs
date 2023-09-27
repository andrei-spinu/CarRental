using System;
using System.ComponentModel.DataAnnotations;
using CarRental.API.Entities;

namespace CarRental.API.Models
{
	public class ReservationsForCarDto
	{
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public decimal DailyRate { get; set; }     
        public bool Available { get; set; }
        public string? Description { get; set; }
        public ICollection<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
    }
}

