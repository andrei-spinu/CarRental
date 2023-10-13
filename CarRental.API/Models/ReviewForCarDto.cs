using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Models
{
	public class ReviewForCarDto
	{
        public int UserId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}

