using RabbitMQ.Client;
using System;
using RabbitMQ.Client.Events;
using System.Text;


namespace AirlineSendAgenet.App
{
    public class AppHost : IAppHost
    {
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
                Console.WriteLine($"[x] Received: {message}");
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