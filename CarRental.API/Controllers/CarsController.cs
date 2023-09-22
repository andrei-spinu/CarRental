
using AutoMapper;
using CarRental.API.Entities;
using CarRental.API.Models;
using CarRental.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository carRepository;
        private readonly IMapper mapper;

        public CarsController(ICarRepository carRepository, IMapper mapper)
        {
            this.carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetAllCars()
        {
            var carsEntities = await this.carRepository.GetCarsAsync();

            return Ok(this.mapper.Map<IEnumerable<CarDto>>(carsEntities));
        }

        [HttpGet("{carid}", Name = "GetCarById")]
        public async Task<ActionResult<CarDto>> GetCarById(int carId)
        {
            if (!await this.carRepository.CarExistsAsync(carId))
            {
                return NotFound();
            }

            var carEntity = await this.carRepository.GetCarByIdAsync(carId);

            return Ok(this.mapper.Map<CarDto>(carEntity));
        }

        [HttpPost]
        public async Task<ActionResult<CarDto>> AddNewCar(NewCarDto newCar)
        {
            if (await this.carRepository.CarRegistrationNumberExistsAsync(newCar.RegistrationNumber))
            {
                return BadRequest($"Car with registration number: {newCar.RegistrationNumber} already exists");
            }

            var finalCarForAdding = this.mapper.Map<Car>(newCar);

            await this.carRepository.AddNewCarAsync(finalCarForAdding);
            await this.carRepository.SaveChangesAsync();

            var carForReturn = this.mapper.Map<CarDto>(finalCarForAdding);

            return CreatedAtRoute("GetCarById", new { carId = carForReturn.Id }, carForReturn);
        }

        [HttpPatch("{carid}")]
        public async Task<ActionResult> PartiallyUpdateCar(int carId, JsonPatchDocument<CarForUpdateDto> patchDocument)
        {
            if(!await this.carRepository.CarExistsAsync(carId))
            {
                return NotFound();
            }

            var carEntity = await this.carRepository.GetCarByIdAsync(carId);
            var carToPatch = this.mapper.Map<CarForUpdateDto>(carEntity);

            patchDocument.ApplyTo(carToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(carToPatch))
            {
                return BadRequest(ModelState);
            }

            this.mapper.Map(carToPatch, carEntity);

            await this.carRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}

