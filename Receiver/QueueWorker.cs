using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;

namespace Receiver
{
    internal class QueueWorker : BackgroundService
    {
        private IModel channel;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true };
            factory.UserName = "guest";
            factory.Password = "guest";

            var queueName = "SampleQueue";

            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Execute;
            channel.BasicConsume(queue: queueName,
                     autoAck: false,
                     consumer: consumer);
            return Task.CompletedTask;
        }

        private Task Execute(object sender, BasicDeliverEventArgs @event)
        {
            var payload = JsonSerializer.Deserialize<Payload>(Encoding.UTF8.GetString(@event.Body.ToArray()));
            Console.WriteLine("Received: " + payload.Message);
            channel.BasicAck(deliveryTag: @event.DeliveryTag, multiple: false);
            return Task.CompletedTask;
        }

    }
}
