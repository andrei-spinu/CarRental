using System;
using CarRental.API.Entities;

namespace CarRental.API.Services
{
	public interface IRentalHistoryRepository
	{
		Task<bool> SaveChangesAsync();
		Task AddNewReservation(int userId, int carId, RentalHistory rentalHistory);

	}
}

