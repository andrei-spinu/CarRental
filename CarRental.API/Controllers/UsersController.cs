using AutoMapper;
using CarRental.API.Entities;
using CarRental.API.Models;
using CarRental.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [ApiController]
	[Route("api/users")]
	public class UsersController : ControllerBase
	{
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUserRepository userRepository, IMapper mapper, ILogger<UsersController> logger)
		{
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserForRegistrationDto>> registerUser(UserForRegistrationDto userForRegistration)
        {
            if(await this.userRepository.UsernameExistsAsync(userForRegistration.Username))
            {
                this.logger.LogWarning($"User whit username: {userForRegistration.Username} already exists");
                return BadRequest($"User whit username: {userForRegistration.Username} already exists");
            }

            if(await this.userRepository.EmailExistsAsync(userForRegistration.Email))
            {
                this.logger.LogWarning($"User with email: {userForRegistration.Email} already exists");
                return BadRequest($"User with email: {userForRegistration.Email} already exists");
            }

            var finalUserForRegistration = this.mapper.Map<User>(userForRegistration);

            await this.userRepository.RegisterUserAsync(finalUserForRegistration);
            await this.userRepository.SaveChangesAsync();

            var createdUserToReturn = this.mapper.Map<UserDto>(finalUserForRegistration);

            return CreatedAtRoute("GetUserById", new { userId = createdUserToReturn.Id }, createdUserToReturn);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var userEntities = await this.userRepository.GetAllUsers();

            return  Ok(this.mapper.Map<IEnumerable<UserDto>>(userEntities));
        }

        [HttpGet("profile/{userid}", Name = "GetUserById")]
        public async Task<ActionResult<UserDto>> GetUserById(int userId)
        {
            var user = await this.userRepository.GetUserById(userId);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(this.mapper.Map<UserDto>(user));
        }

        [HttpPut("profile/{userid}")]
        public async Task<ActionResult> UpdateUser(int userId, UserForUpdateDto userForUpdate)
        {
            if(!await this.userRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var userEntity = await this.userRepository.GetUserById(userId);

            this.mapper.Map(userForUpdate, userEntity);

            await this.userRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("profile/{userid}")]
        public async Task<ActionResult> PartiallyUpdateUser(int userId, JsonPatchDocument<UserForUpdateDto> patchDocument)
        {
            if(!await this.userRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var userEntity = await this.userRepository.GetUserById(userId);

            var userToPatch = this.mapper.Map<UserForUpdateDto>(userEntity);

            patchDocument.ApplyTo(userToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(userToPatch))
            {
                return BadRequest(ModelState);
            }

            this.mapper.Map(userToPatch, userEntity);

            await this.userRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("profile/{userid}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            if(!await this.userRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var userEntity = await this.userRepository.GetUserById(userId);

            this.userRepository.DeleteUser(userEntity);

            await this.userRepository.SaveChangesAsync();

            return NoContent();
        }
	}
}

