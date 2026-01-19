using AirlineWeb.Dtos;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AirlineWeb.MessageBus
{

    public class MessageBusClient : IMessageBusClient
    {
        public void PublishNotification(NotificationMessageDto notificationMessage)
        {
            // RabbitMQ connection settings
            var factory = new ConnectionFactory()
            {
                HostName = "localhost", // Change to your RabbitMQ host
                UserName = "guest",     // Default username
                Password = "guest",     // Default password
                Port = 5672,            // Default port
                VirtualHost = "/",      // Default vhost
                ConsumerDispatchConcurrency = 1  // For async consumers
            };

            try
            {
                // Create connection (async in .NET 6+)
                using var connection = factory.CreateConnectionAsync().Result;

                // Create channel
                using var channel = connection.CreateChannelAsync().Result;

                Console.WriteLine("✅ RabbitMQ connection and channel created successfully.");

                channel.ExchangeDeclareAsync(exchange: "airline_exchange", type: ExchangeType.Direct).Wait();


                // Example: Publish a message
                var message = notificationMessage.ToString();
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublishAsync(exchange: "airline_exchange",
                    routingKey: "",
                    mandatory: false,
                    basicProperties: new BasicProperties { ContentType = "text/plain" },
                    body: body,
                    cancellationToken: CancellationToken.None);

                Console.WriteLine($"📨 Sent: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}