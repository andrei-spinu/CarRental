using System;
using CarRental.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace CarRental.API.Models
{
	public class ReviewsFromUserDto
	{
        public int Id { get; set; }
        public string Username { get; set; }
        public int NumberOfReviews
        {
            get
            {
                return Reviews.Count;
            }
        }
        public ICollection<ReviewFromUserDto> Reviews { get; set; } = new List<ReviewFromUserDto>();

        
    }
}

