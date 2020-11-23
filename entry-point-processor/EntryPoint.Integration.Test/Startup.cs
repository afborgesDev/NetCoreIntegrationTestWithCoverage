using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQSharedConfigurations;
using System.IO;

namespace EntryPoint.Integration.Test
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) =>
           hostBuilder
               .ConfigureServices((context, services) =>
               {
                   var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", true);
                   context.Configuration = builder.Build();
                   services.RegisterRabbitMQ(context.Configuration);
                   var rabbitConnection = EasyNetQConfigurationBuilder.Build(context.Configuration);
                   services.AddSingleton<ConnectionConfiguration>(rabbitConnection);
               });
    }
}