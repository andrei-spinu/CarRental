using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities
{
	[Index(nameof(RegistrationNumber), IsUnique =true)]
	public class Car
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

		public ICollection<RentalHistory> Reservations { get; set; } = new List<RentalHistory>();
		public ICollection<Review> Reviews { get; set; } = new List<Review>();
	}
}

