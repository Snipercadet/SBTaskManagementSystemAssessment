using Microsoft.AspNetCore.Hosting;
using SBTaskManagement.Infrastructure;
using TaskProcessingWorker;
using TaskProcessingWorker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    }).ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.RegisterServices();
        services.RegisterExternalServices();
        services.ConfigureAutoMapper();
        services.AddScoped<ITaskProcessingCron, TaskProcessingCron>();
        
    })
    .Build();

await host.RunAsync();
