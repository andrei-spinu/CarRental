using System;
using CarRental.API.Entities;
using CarRental.API.Models;

namespace CarRental.API.Services
{
	public interface ICarRepository
	{
		Task AddNewCarAsync(Car car);
		Task<bool> SaveChangesAsync();
		Task<IEnumerable<Car>> GetCarsAsync();
		Task<Car> GetReservationsForCarAsync(int carId);
        Task<IEnumerable<Car>> GetReservationsForCarsAsync();
        Task<IEnumerable<Car>> GetAvailableCarsForDateRange(NewReservationDto newReservationDto);
		Task<Car> GetCarByIdAsync(int carId);
		Task<bool> CarExistsAsync(int carId);
		Task<bool> CarRegistrationNumberExistsAsync(string registrationNumber);
		Task<bool> IsCarAvailableForDateRange(int carId, NewReservationDto newReservationDto);

    }
}

