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
        private readonly ILogger<RentalHistoryController> logger;

        public RentalHistoryController(IRentalHistoryRepository rentalHistoryRepository,
            ICarRepository carRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<RentalHistoryController> logger)
        {
            this.rentalHistoryRepository = rentalHistoryRepository ?? throw new ArgumentNullException(nameof(rentalHistoryRepository));
            this.carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<ActionResult<RentalHistory>> CreateNewReservation(int userId, int carId, NewReservationDto newReservation)
        {
            if (!await this.userRepository.UserExistsAsync(userId))
            {
                string message = $"User with id {userId} wasn't found when accessing reservations.";
                this.logger.LogInformation(message);
                return base.NotFound(message);
            }

            if (!await this.carRepository.CarExistsAsync(carId))
            {
                string message = $"Car with id {carId} wasn't found when accessing reservations.";
                this.logger.LogInformation(message);
                return NotFound(message);
            }

            if(!await this.carRepository.IsCarAvailableForDateRange(carId, newReservation))
            {
                string message = $"Car with {carId} is not available between {newReservation.StartDate} and {newReservation.EndDate}";
                this.logger.LogInformation(message);
                return BadRequest(message);
            }

            var car = await this.carRepository.GetCarByIdAsync(carId);

            var finalReservation = this.mapper.Map<NewReservationDto, RentalHistory>(newReservation, opts => opts.Items["DailyRate"] = car.DailyRate);
            finalReservation.Status = "Pending";

            await this.rentalHistoryRepository.AddNewReservationAsync(userId, carId, finalReservation);
            await this.rentalHistoryRepository.SaveChangesAsync();
           
            var reservationToReturn = this.mapper.Map<ReservationDto>(finalReservation);

            return CreatedAtRoute("GetReservationForCarAndUser", new {userId = userId, carId = carId, reservationId = reservationToReturn.Id}, reservationToReturn);
        }

        [HttpGet("{reservationid}", Name = "GetReservationForCarAndUser")]
        public async Task<ActionResult<ReservationDto>> GetReservationForCarAndUser(int userId, int carId, int reservationId)
        {
            if(!await this.userRepository.UserExistsAsync(userId))
            {
                this.logger.LogInformation($"User with id {userId} wasn't found when accessing reservations.");
                return NotFound();
            }

            if(!await this.carRepository.CarExistsAsync(carId))
            {
                this.logger.LogInformation($"Car with id {carId} wasn't found when accessing reservations.");
                return NotFound();
            }

            var reservation = await this.rentalHistoryRepository.GetReservationForCarAndUserAsync(userId, carId, reservationId);

            if(reservation == null)
            {
                this.logger.LogInformation($"Reservation with id {reservationId} wasn't found when accessing reservations.");
                return NotFound($"Reservation with id {reservationId} wasn't found when accessing reservations.");
            }

            return Ok(this.mapper.Map<ReservationDto>(reservation));
         
        }

        [HttpPut("{reservationid}/cancel")]
        public async Task<ActionResult> CancelReservationForCarAndUser(int userId, int carId, int reservationId)
        {
            if (!await this.userRepository.UserExistsAsync(userId))
            {
                this.logger.LogInformation($"User with id {userId} wasn't found when accessing reservations.");
                return NotFound();
            }

            if (!await this.carRepository.CarExistsAsync(carId))
            {
                this.logger.LogInformation($"Car with id {carId} wasn't found when accessing reservations.");
                return NotFound();
            }

            var reservationEntity = await this.rentalHistoryRepository.GetReservationForCarAndUserAsync(userId, carId, reservationId);

            if (reservationEntity == null)
            {
                this.logger.LogInformation($"Reservation with id {reservationId} wasn't found when accessing reservations.");
                return NotFound($"Reservation with id {reservationId} wasn't found when accessing reservations.");
            }

            reservationEntity.Status = "Canceled";

            await this.rentalHistoryRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{reservationid}/confirm")]
        public async Task<ActionResult> ConfirmReservationForCarAndUser(int userId, int carId, int reservationId)
        {
            if (!await this.userRepository.UserExistsAsync(userId))
            {
                this.logger.LogInformation($"User with id {userId} wasn't found when accessing reservations.");
                return NotFound();
            }

            if (!await this.carRepository.CarExistsAsync(carId))
            {
                this.logger.LogInformation($"Car with id {carId} wasn't found when accessing reservations.");
                return NotFound();
            }

            var reservationEntity = await this.rentalHistoryRepository.GetReservationForCarAndUserAsync(userId, carId, reservationId);

            if (reservationEntity == null)
            {
                this.logger.LogInformation($"Reservation with id {reservationId} wasn't found when accessing reservations.");
                return NotFound($"Reservation with id {reservationId} wasn't found when accessing reservations.");
            }

            reservationEntity.Status = "Confirmed";

            await this.rentalHistoryRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}

