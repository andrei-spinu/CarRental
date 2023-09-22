using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.API.Entities
{
	public class RentalHistory
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("UserId")]
		public User? User { get; set; }
		public int UserId { get; set; }

		[ForeignKey("CarId")]
		public Car? Car { get; set; }
		public int CarId { get; set; }

		[Required]
		[Column(TypeName = "date")]
		public DateTime StartDate { get; set; }
		[Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

		public decimal TotalCost { get; set; }
		public string Status { get; set; }

	}
}

