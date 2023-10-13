using System;
using AutoMapper;

namespace CarRental.API.Profiles
{
	public class ReviewProfile : Profile
	{
		public ReviewProfile()
		{
			CreateMap<Models.NewReviewDto, Entities.Review>();
			CreateMap<Entities.Review, Models.ReviewDto>();
			CreateMap<Entities.Review, Models.ReviewForCarDto>();
			CreateMap<Entities.Review, Models.ReviewFromUserDto>();
		}
	}
}

