using System;
using CarRental.API.Entities;

namespace CarRental.API.Services
{
	public interface IReviewRepository
	{
        Task<bool> SaveChangesAsync();
        Task AddNewReviewAsync(int userId, int carId, Review review);
        Task<Review?> GetReviewForCarAndUserAsync(int userId, int carId, int reviewId);
        Task<Car> GetAllReviewsForCar(int carId);
        Task<User> GetReviewsByUserId(int userId);        
    }
}

