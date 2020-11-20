using System.IO;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQSharedConfigurations;

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

        //public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        //{
        //    context.Configuration.
        //}

        //public async Task ConfigureHost(IHostBuilder hostbuilder) =>
        //    await hostbuilder.ConfigureWebHost(webHostbuilder =>
        //        webHostbuilder.ConfigureAppConfiguration((context, builder) =>
        //                      {
        //                          builder.SetBasePath(Directory.GetCurrentDirectory());
        //                          builder.AddJsonFile("appsettings.json", true);
        //                      })
        //                      .ConfigureServices((context, services) =>
        //                      {
        //                          services.RegisterRabbitMQ(context.Configuration);
        //                      })).RunConsoleAsync();
    }
}

/*

   configHost.SetBasePath(Directory.GetCurrentDirectory());
                          configHost.AddJsonFile("appsettings.json", true);

  .ConfigureServices((hostContext, services) =>
                      {
                          services.AddOptions();
                          services.AddTransient<IWorkerService, WorkerService>();
                          services.RegisterRabbitMQ(hostContext.Configuration);
                          services.AddHostedService<WorkerHostedService>();
                      })
 */
