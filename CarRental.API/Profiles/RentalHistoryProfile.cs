using System;
using AutoMapper;

namespace CarRental.API.Profiles
{
	public class RentalHistoryProfile : Profile
	{
        public RentalHistoryProfile()
        {

            CreateMap<Models.NewReservationDto, Entities.RentalHistory>()
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom((src, dest, _, context) =>
                {
                    if (context.Items.TryGetValue("DailyRate", out var dailyRateValue) && dailyRateValue is decimal dailyRate)
                    {
                        int numberOfDays = (src.EndDate - src.StartDate).Days;
                        decimal totalCost = numberOfDays * dailyRate;
                        return totalCost;
                    }
                    else
                    { 
                        throw new InvalidOperationException("DailyRate not found in context.");
                    }
                }));
            CreateMap<Entities.RentalHistory, Models.ReservationDto>();

        }
    }
}

