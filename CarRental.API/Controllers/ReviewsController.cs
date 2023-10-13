using System.Security.Claims;
using AutoMapper;
using CarRental.API.Entities;
using CarRental.API.Models;
using CarRental.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository reviewRepository;
        private readonly ICarRepository carRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ReviewsController> logger;

        public ReviewsController(IReviewRepository reviewRepository, ICarRepository carRepository, IUserRepository userRepository, IMapper mapper, ILogger<ReviewsController> logger)
        {
            this.reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
            this.carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize]
        [HttpPost("users/{userId}/cars/{carId}")]
        public async Task<ActionResult<ReviewDto>> AddReviewForCarFromUser(int userId, int carId, NewReviewDto newReview)
        {
            var userIdFromToken = HttpContext.User.Claims.FirstOrDefault(user => user.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var userRoleFromToken = HttpContext.User.Claims.FirstOrDefault(user => user.Type.Equals(ClaimTypes.Role))?.Value;

            if (!userIdFromToken.Equals(userId.ToString()) && userRoleFromToken.Equals("USER"))
            {
                return Forbid();
            }

            if (!await this.userRepository.UserExistsAsync(userId))
            {
                string message = $"User with id {userId} wasn't found when accessing reviews.";
                this.logger.LogInformation(message);
                return base.NotFound(message);
            }

            if (!await this.carRepository.CarExistsAsync(carId))
            {
                string message = $"Car with id {carId} wasn't found when accessing reviews.";
                this.logger.LogInformation(message);
                return NotFound(message);
            }

            var finalReviewToAdd = this.mapper.Map<Review>(newReview);

            finalReviewToAdd.ReviewDate = DateTime.Now;

            await this.reviewRepository.AddNewReviewAsync(userId, carId, finalReviewToAdd);
            await this.reviewRepository.SaveChangesAsync();

            var reviewToReturn = this.mapper.Map<ReviewDto>(finalReviewToAdd);

            return CreatedAtRoute("GetReviewByUserAndCarId", new { userId = userId, carId = carId, reviewId = reviewToReturn.Id }, reviewToReturn);


        }

        [HttpGet("{reviewId}/users/{userId}/cars/{carId}", Name = "GetReviewByUserAndCarId")]
        public async Task<ActionResult<ReviewDto>> GetReviewByUserAndCarId( int userId, int carId, int reviewId)
        {
            if (!await this.userRepository.UserExistsAsync(userId))
            {
                string message = $"User with id {userId} wasn't found when accessing reviews.";
                this.logger.LogInformation(message);
                return base.NotFound(message);
            }

            if (!await this.carRepository.CarExistsAsync(carId))
            {
                string message = $"Car with id {carId} wasn't found when accessing reviews.";
                this.logger.LogInformation(message);
                return NotFound(message);
            }

            var reviewEntity = await this.reviewRepository.GetReviewForCarAndUserAsync(userId, carId, reviewId);

            if (reviewEntity == null)
            {
                string message = $"Review with id {reviewId} wasn't found when accessing reviews.";
                this.logger.LogInformation(message);
                return base.NotFound(message);
            }

            return Ok(this.mapper.Map<ReviewDto>(reviewEntity));

        }

        [HttpGet("cars/{carId}")]
        public async Task<ActionResult<ReviewsForCarDto>> GetAllReviewsForCar(int carId)
        {
            if (!await this.carRepository.CarExistsAsync(carId))
            {
                string message = $"Car with id {carId} wasn't found when accessing reviews.";
                this.logger.LogInformation(message);
                return NotFound(message);
            }

            var carEntity =await this.reviewRepository.GetAllReviewsForCar(carId);



            return Ok(this.mapper.Map<ReviewsForCarDto>(carEntity));
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<ReviewsFromUserDto>> GetAllReviewsFromUser(int userId)
        {
            if (!await this.userRepository.UserExistsAsync(userId))
            {
                string message = $"User with id {userId} wasn't found when accessing reviews.";
                this.logger.LogInformation(message);
                return base.NotFound(message);
            }

            var userEntity = await this.reviewRepository.GetReviewsByUserId(userId);

            return Ok(this.mapper.Map<ReviewsFromUserDto>(userEntity));
        }
    }
}

