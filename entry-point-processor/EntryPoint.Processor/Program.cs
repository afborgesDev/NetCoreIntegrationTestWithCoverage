using System.IO;
using System.Threading.Tasks;
using EntryPoint.Processor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQSharedConfigurations;

namespace EntryPoint.Processor
{
    internal class Program
    {
        private static async Task Main(string[] args) =>
            await Host.CreateDefaultBuilder(args)
                      .ConfigureHostConfiguration(configHost =>
                      {
                          configHost.SetBasePath(Directory.GetCurrentDirectory());
                          configHost.AddJsonFile("appsettings.json", true);
                          configHost.AddCommandLine(args);
                      })
                      .ConfigureServices((hostContext, services) =>
                      {
                          services.AddOptions();
                          services.AddTransient<IWorkerService, WorkerService>();
                          services.RegisterRabbitMQ(hostContext.Configuration);
                          services.AddHostedService<WorkerHostedService>();
                      })
                      .UseConsoleLifetime()
                      .RunConsoleAsync()
                      .ConfigureAwait(false);
    }
}
