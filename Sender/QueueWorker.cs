using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    internal class QueueWorker : BackgroundService
    {
        private Timer timer;
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            timer = new Timer(OnTimer, null, 0, 5000);            
            return Task.CompletedTask;
        }

        private void OnTimer(object? state)
        {
            var payload = $"on timer {DateTime.Now}";

            var queueName = "SampleQueue";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            factory.UserName = "guest";
            factory.Password = "guest";
            //factory.Port = settings.RabbitMqPort;

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    var body = Encoding.UTF8.GetBytes(payload);
                    channel.BasicPublish(exchange: string.Empty,
                        routingKey: queueName,
                        basicProperties: properties,
                        body: body);
                }
            }

        }
    }
}
