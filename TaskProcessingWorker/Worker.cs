using Microsoft.AspNetCore.Builder;
using SBTaskManagement.Application.Services.Interfaces;
using TaskProcessingWorker.Services;

namespace TaskProcessingWorker
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string errorMessage = "An error occurred on the background job at";
            while (true)
            {
                try
                {
                    using (var scope  = _serviceProvider.CreateScope())
                    {
                        var scopedService = scope.ServiceProvider.GetRequiredService<ITaskProcessingCron>();
                        await scopedService.ProcessTask();
                    }
                    
                    await Task.Delay(2000, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(errorMessage + " " + $"{DateTime.UtcNow.ToString()} {e.Message}");
                }
            }
        }
    }
}