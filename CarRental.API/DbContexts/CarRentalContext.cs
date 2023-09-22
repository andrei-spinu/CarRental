using System;
using CarRental.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.DbContexts
{
	public class CarRentalContext : DbContext
	{
		public DbSet<Car> Cars { get; set; } = null!;
		public DbSet<RentalHistory> Reservations { get; set; } = null!;
		public DbSet<Review> Reviews { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;
		public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options)
		{
	
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

