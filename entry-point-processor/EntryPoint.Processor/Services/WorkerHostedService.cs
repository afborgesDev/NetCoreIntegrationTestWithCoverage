using System;
using System.Threading;
using System.Threading.Tasks;
using EntryPoint.Processor.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class WorkerHostedService : IHostedService
{
    private readonly ILogger<WorkerHostedService> logger;
    private readonly IHostApplicationLifetime appLifetime;
    private readonly IWorkerService workerService;

    public WorkerHostedService(ILogger<WorkerHostedService> logger,
        IHostApplicationLifetime appLifetime,
        IWorkerService workerService)
    {
        this.logger = logger;
        this.appLifetime = appLifetime;
        this.workerService = workerService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Starting EntryPoint worker service");

        this.appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    await this.workerService.ConsumeAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Unhandled exception!");
                }
            }).ConfigureAwait(false);
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
