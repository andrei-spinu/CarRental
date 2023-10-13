using System;
using System.ComponentModel.DataAnnotations;
using CarRental.API.Entities;

namespace CarRental.API.Models
{
	public class ReviewsForCarDto
	{
        [Required]
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        public int NumberOfReviews
        {
            get
            {
                return Reviews.Count;
            }
        }
        public ICollection<ReviewForCarDto> Reviews { get; set; } = new List<ReviewForCarDto>();
    }
}

