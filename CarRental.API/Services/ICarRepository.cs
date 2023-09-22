using System;
using CarRental.API.Entities;

namespace CarRental.API.Services
{
	public interface ICarRepository
	{
		Task AddNewCarAsync(Car car);
		Task<bool> SaveChangesAsync();
		Task<IEnumerable<Car>> GetCarsAsync();
		Task<Car> GetCarByIdAsync(int carId);
		Task<bool> CarExistsAsync(int carId);
		Task<bool> CarRegistrationNumberExistsAsync(string registrationNumber);
	}
}

