using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sender;

Console.WriteLine("System started");

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Rabbit MQ sender";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<QueueWorker>();
    })
    .Build();

await host.RunAsync();