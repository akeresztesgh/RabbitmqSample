// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Receiver;

Console.WriteLine("System started");

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Rabbit MQ Receiver";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<QueueWorker>();
    })
    .Build();

await host.RunAsync();

