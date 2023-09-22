using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Models
{
	public class UserForUpdateDto
	{
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Address { get; set; }

    }
}

