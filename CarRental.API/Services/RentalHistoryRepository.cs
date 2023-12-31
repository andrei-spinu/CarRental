﻿using System;
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

        public async Task AddNewReservationAsync(int userId, int carId, RentalHistory rentalHistory)
        {
            var user = await this.context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var car = await this.context.Cars
                .Where(c => c.Id == carId)
                .FirstOrDefaultAsync();

            if(car!=null && user != null)
            {
                user.Reservations.Add(rentalHistory);
                car.Reservations.Add(rentalHistory);
            }
        }

        public async Task<RentalHistory?> GetReservationForCarAndUserAsync(int userId, int carId, int reservationId)
        {
            return await this.context
                .Reservations
                .Where(r => r.CarId == carId && r.UserId == userId && r.Id == reservationId)
                .FirstOrDefaultAsync();
        }



        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync() > 0);
        }

        public async Task UpdateActiveReservations()
        {
            var reservationsToActivate = await this.context
                .Reservations
                .Where(r => r.Status == "Confirmed" && r.StartDate < DateTime.Now)
                .ToListAsync();

            foreach(var reservation in reservationsToActivate)
            {
                reservation.Status = "Active";
            }

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateCompletedReservations()
        {
            var reservationsToActivate = await this.context
                .Reservations
                .Where(r => r.Status == "Active" && r.EndDate < DateTime.Now)
                .ToListAsync();

            foreach (var reservation in reservationsToActivate)
            {
                reservation.Status = "Completed";
            }

            await this.context.SaveChangesAsync();
        }
    }
}

