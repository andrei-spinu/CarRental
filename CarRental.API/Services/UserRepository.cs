using System;
using CarRental.API.DbContexts;
using CarRental.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly CarRentalContext context;

        public UserRepository(CarRentalContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await this.context.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await this.context.Users
                .Where(user => user.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task RegisterUserAsync(User user)
        {
            await this.context.AddAsync(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync() > 0);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await this.context
                .Users
                .AnyAsync(user => user.Username.Equals(username));
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await this.context
                 .Users
                 .AnyAsync(user => user.Email.Equals(email));
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await this.context
                .Users
                .AnyAsync(user => user.Id == id);
        }

        public void DeleteUser(User user)
        {
            this.context.Remove(user);
        }
    }
}

