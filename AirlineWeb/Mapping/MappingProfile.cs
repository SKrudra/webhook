using AutoMapper;
using AirlineWeb.Models;

namespace AirlineWeb.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WebhookSubscription, WebhookSubscriptionDto>().ReverseMap();
            CreateMap<CreateWebhookSubscriptionDto, WebhookSubscription>();
        }
    }
}