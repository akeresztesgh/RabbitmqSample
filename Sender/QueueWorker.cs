using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

namespace Sender
{
    internal class QueueWorker : BackgroundService
    {
        private const string QueueName = "SampleQueue";
        private Timer timer;
        private IConnection connection;
        private IModel channel;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            factory.UserName = "guest";
            factory.Password = "guest";
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: QueueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false);

            timer = new Timer(OnTimer, null, 0, 5000);
            return Task.CompletedTask;
        }

        private void OnTimer(object? state)
        {
            var payload = new Payload()
            {
                Message = $"on timer {DateTime.Now}"
            };

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
            channel.BasicPublish(exchange: string.Empty,
                routingKey: QueueName,
                basicProperties: properties,
                body: body);
        }

        ~QueueWorker()
        {
            channel.Close();
            connection.Close();
            channel.Dispose();
            connection.Dispose();
        }
    }
}
