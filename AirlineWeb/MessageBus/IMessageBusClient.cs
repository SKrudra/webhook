using AirlineWeb.Dtos;

namespace AirlineWeb.MessageBus
{

    public interface IMessageBusClient
    {
        void PublishNotification(NotificationMessageDto notificationMessage);
    }
}