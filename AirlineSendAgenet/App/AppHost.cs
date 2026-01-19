using RabbitMQ.Client;
using System;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using AirlineSendAgenet.Dtos;
using AirlineSendAgenet.Client;
using AirlineSendAgenet.Data;


namespace AirlineSendAgenet.App
{
    public class AppHost : IAppHost
    {
        // add constructor to accept IWebhookClient and dbcontext
        private readonly IWebhookClient _webhookClient;
        private readonly AirlineDbContext _dbContext;
        public AppHost(IWebhookClient webhookClient, AirlineDbContext dbContext)
        {
            _webhookClient = webhookClient;
            _dbContext = dbContext;
        }

        public void Run()
        {
            Console.WriteLine("AppHost is running...");
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
            using var connection = factory.CreateConnectionAsync().Result;
            using var channel = connection.CreateChannelAsync().Result;
            channel.ExchangeDeclareAsync(exchange: "airline_exchange", type: ExchangeType.Direct).Wait();
            // get the dynamic queue name
            var queueName = channel.QueueDeclareAsync().Result.QueueName;
            channel.QueueBindAsync(queue: queueName,
                                 exchange: "airline_exchange",
                                 routingKey: "").Wait();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                // deserialize the message into NotificationMessageDto from json
                var notificationMessage = JsonSerializer.Deserialize<NotificationMessageDto>(message);
                Console.WriteLine($"[x] Received: {message}");

                var payload = new FlightDetailsChangePayloadDto
                {
                    Publisher = string.Empty,
                    Secret = string.Empty,
                    WebhookUrl = string.Empty,
                    FlightNumber = notificationMessage.FlightNumber,
                    WebhookType = notificationMessage.WebhookType,
                    OldPrice = notificationMessage.OldPrice,
                    NewPrice = notificationMessage.NewPrice,
                };

                foreach (var subscription in _dbContext.WebhookSubscriptions.Where(ws => ws.WebhookType == notificationMessage.WebhookType))
                {
                    payload.WebhookUrl = subscription.WebhookUrl;
                    payload.Secret = subscription.Secret;
                    payload.Publisher = subscription.WebhookPublisher;
                    await _webhookClient.SendNotificationAsync(payload);
                }

            };

            // Start consuming messages
            channel.BasicConsumeAsync(queue: queueName,
            autoAck: true,
            consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}