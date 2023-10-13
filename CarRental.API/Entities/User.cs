using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Entities
{
	[Index(nameof(Username),IsUnique = true)]
	[Index(nameof(Email), IsUnique = true)]
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MinLength(6)]
		[MaxLength(16)]
        public string Username { get; set; }
		[Required]
		[MinLength(8)]
		[MaxLength(32)]
		public string Password { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		public string? Address { get; set; }
		public string? Role { get; set; }

		public ICollection<RentalHistory> Reservations { get; set; } = new List<RentalHistory>();
		public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public User(string username, string password, string email, string firstName, string lastName)
        {
            Username = username;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}

