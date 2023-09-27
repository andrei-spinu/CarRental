using System;
using AutoMapper;

namespace CarRental.API.Profiles
{
	public class CarProfile : Profile
	{
		public CarProfile()
        {
			CreateMap<Models.NewCarDto, Entities.Car>();
            CreateMap<Entities.Car, Models.CarDto>();
			CreateMap<Entities.Car, Models.CarForUpdateDto>();
			CreateMap<Models.CarForUpdateDto, Entities.Car>();
			CreateMap<Entities.Car, Models.ReservationsForCarDto>();
		}
	}
}

