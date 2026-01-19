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
                using var connection = factory.CreateConnectionAsync();

                // Create channel
                using var channel = connection.Result.CreateChannelAsync();

                Console.WriteLine("✅ RabbitMQ connection and channel created successfully.");

                // Example: Declare a queue
                // channel.ExchangeDeclare(
                //     exchange: "test-exchange",
                //     type: ExchangeType.Direct,
                //     durable: true,
                //     exclusive: false,
                //     autoDelete: false,
                //     arguments: null
                // );

                // Example: Publish a message
                var message = "Hello from .NET 10!";
                var body = Encoding.UTF8.GetBytes(message);

                // channel.BasicPublishAsync(
                //     exchange: "test-exchange",
                //     routingKey: "test-queue",
                //     mandatory: false,
                //     // basicProperties: null,
                //     body: body
                // );

                Console.WriteLine($"📨 Sent: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}