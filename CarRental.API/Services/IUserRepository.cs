using System;
using CarRental.API.Entities;

namespace CarRental.API.Services
{
	public interface IUserRepository
	{
		Task RegisterUserAsync(User user);
		Task<bool> SaveChangesAsync();
		Task<IEnumerable<User>> GetAllUsers();
		Task<User> GetUserById(int id);
		Task<bool> EmailExistsAsync(string email);
		Task<bool> UsernameExistsAsync(string username);
		Task<bool> UserExistsAsync(int id);
		void DeleteUser(User user);
	}
}

