using System;
using CarRental.API.DbContexts;
using CarRental.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class RentalHistoryRepository : IRentalHistoryRepository
	{
        private readonly CarRentalContext context;

        public RentalHistoryRepository(CarRentalContext context)
		{
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddNewReservation(int userId, int carId, RentalHistory rentalHistory)
        {
            var user = await this.context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var car = await this.context.Cars
                .Where(c => c.Id == carId)
                .FirstOrDefaultAsync();

            Console.WriteLine(rentalHistory.UserId+"-------------------42342342343243242344324234234");

            if(car!=null && user != null)
            {
                user.Reservations.Add(rentalHistory);
                car.Reservations.Add(rentalHistory);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync() > 0);
        }
    }
}

