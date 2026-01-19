using System;
using AirlineSendAgenet.App;
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
                })
                .Build();

            var appHost = host.Services.GetRequiredService<IAppHost>();
            appHost.Run();
        }
    }
}
