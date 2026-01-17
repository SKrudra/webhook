using AutoMapper;
using AirlineWeb.Models;
using AirlineWeb.Dtos;

namespace AirlineWeb.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WebhookSubscription, WebhookSubscriptionDto>().ReverseMap();
            CreateMap<CreateWebhookSubscriptionDto, WebhookSubscription>();
            CreateMap<CreateFlightDetailsDto, FlightDetails>();
            CreateMap<UpdateFlightDetailsDto, FlightDetails>();
            CreateMap<FlightDetails, FlightDetailsDto>().ReverseMap();
        }
    }
}