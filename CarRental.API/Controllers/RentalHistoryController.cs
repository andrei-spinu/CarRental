using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.API.Entities;
using CarRental.API.Models;
using CarRental.API.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarRental.API.Controllers
{
    [ApiController]
    [Route("api/users/{userid}/cars/{carid}/rentalhistory")]
    public class RentalHistoryController : ControllerBase
    {
        private readonly IRentalHistoryRepository rentalHistoryRepository;
        private readonly ICarRepository carRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public RentalHistoryController(IRentalHistoryRepository rentalHistoryRepository,
            ICarRepository carRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.rentalHistoryRepository = rentalHistoryRepository ?? throw new ArgumentNullException(nameof(rentalHistoryRepository));
            this.carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<ActionResult<RentalHistory>> CreateNewReservation(int userId, int carId, NewReservationDto newReservation)
        {
            if(!await this.userRepository.UserExistsAsync(userId))
            {
                return NotFound("User");
            }
            if(!await this.carRepository.CarExistsAsync(carId))
            {
                return NotFound("Car");
            }

            var car = await this.carRepository.GetCarByIdAsync(carId);

            var finalReservation = this.mapper.Map<NewReservationDto, RentalHistory>(newReservation, opts => opts.Items["DailyRate"] = car.DailyRate);
            finalReservation.Status = "Pending";
            //return Ok(finalReservation);

            await this.rentalHistoryRepository.AddNewReservation(userId, carId, finalReservation);
            await this.rentalHistoryRepository.SaveChangesAsync();


            return Ok();
        }
    }
}

