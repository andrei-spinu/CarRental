using System;
using CarRental.API.Entities;

namespace CarRental.API.Services
{
	public interface IRentalHistoryRepository
	{
		Task<bool> SaveChangesAsync();
		Task AddNewReservationAsync(int userId, int carId, RentalHistory rentalHistory);
		Task<RentalHistory?> GetReservationForCarAndUserAsync(int userId, int carId, int reservationId);

	}
}

