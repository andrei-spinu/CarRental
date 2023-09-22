using System;
using CarRental.API.DbContexts;
using CarRental.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class CarRepository : ICarRepository
	{
        private readonly CarRentalContext context;

        public CarRepository(CarRentalContext context)
		{
            this.context = context;
        }

        public async Task AddNewCarAsync(Car car)
        {
            await this.context.AddAsync(car);
        }


        public async Task<Car?> GetCarByIdAsync(int carId)
        {
            return await this.context
                .Cars
                .Where(car => car.Id == carId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsAsync()
        {
            return await this.context.Cars.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync() > 0);
        }

        public async Task<bool> CarExistsAsync(int carId)
        {
            return await this.context
                .Cars
                .AnyAsync(car => car.Id == carId);
        }

        public async Task<bool> CarRegistrationNumberExistsAsync(string registrationNumber)
        {
            return await this.context.Cars.AnyAsync(car => car.RegistrationNumber.Equals(registrationNumber));
        }
    }
}

