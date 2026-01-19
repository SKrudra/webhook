using System;
using AirlineSendAgenet.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirlineSendAgenet
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IAppHost, AppHost>();
                    services.AddSingleton<Client.IWebhookClient, Client.WebhookClient>();
                    services.AddHttpClient();
                    services.AddDbContext<Data.AirlineDbContext>(options =>
                    {
                        var conn = context.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'");
                        options.UseSqlServer(conn);
                    });
                })
                .Build();

            var appHost = host.Services.GetRequiredService<IAppHost>();
            appHost.Run();
        }
    }
}
