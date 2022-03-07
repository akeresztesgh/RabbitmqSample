// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Receiver;

Console.WriteLine("System started");

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ".NET Joke Service";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<QueueWorker>();
    })
    .Build();

await host.RunAsync();

