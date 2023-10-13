using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Models
{
	public class NewReviewDto
	{
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

