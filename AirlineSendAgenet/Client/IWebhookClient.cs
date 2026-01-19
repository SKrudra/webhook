using AirlineSendAgenet.Dtos;

namespace AirlineSendAgenet.Client
{
    public interface IWebhookClient
    {
        Task SendNotificationAsync(FlightDetailsChangePayloadDto payload);
    }
}