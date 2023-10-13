using System;
using CarRental.API.DbContexts;
using CarRental.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class ReviewRepository : IReviewRepository
	{
        private readonly CarRentalContext context;

        public ReviewRepository(CarRentalContext context)
		{
            this.context = context;
        }

        public async Task AddNewReviewAsync(int userId, int carId, Review review)
        {
            var user = await this.context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var car = await this.context.Cars
                .Where(c => c.Id == carId)
                .FirstOrDefaultAsync();

            if (car != null && user != null)
            {
                user.Reviews.Add(review);
                car.Reviews.Add(review);
            }
        }

        public async Task<Car?> GetAllReviewsForCar(int carId)
        {
            return await this.context
                .Cars
                .Include(car => car.Reviews)
                .Where(car => car.Id == carId)
                .FirstOrDefaultAsync();
        }

        public async Task<Review?> GetReviewForCarAndUserAsync(int userId, int carId, int reviewId)
        {
            return await this.context
                .Reviews
                .Where(review => review.CarId == carId && review.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetReviewsByUserId(int userId)
        {
            return await this.context
                .Users
                .Include(user => user.Reviews)
                .Where(user => user.Id == userId)
                .FirstOrDefaultAsync();

        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync() > 0);
        }
    }
}

