using System;
using AutoMapper;

namespace CarRental.API.Profiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<Models.UserForRegistrationDto, Entities.User>();
			CreateMap<Entities.User, Models.UserDto>();
			CreateMap<Entities.User, Models.UserForUpdateDto>();
			CreateMap<Models.UserForUpdateDto, Entities.User>();
			CreateMap<Entities.User, Models.ReviewsFromUserDto>();
		}
	}
}

