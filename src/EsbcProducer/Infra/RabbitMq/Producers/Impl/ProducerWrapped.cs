using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EsbcProducer.Infra.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EsbcProducer.Infra.RabbitMq.Producers.Impl
{
    public class ProducerWrapped : IRabbitMqProducerWrapped
    {
        private readonly QueueConfiguration _queueConfiguration;

        public ProducerWrapped(IOptions<QueueConfiguration> queueConfiguration)
        {
            _queueConfiguration = queueConfiguration.Value;
        }

        public Task<bool> Send(string queueName, string payload, CancellationToken stoppingToken = default)
        {
            return Task.Run<bool>(() =>
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = _queueConfiguration.HostName,
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest",
                };

                using var connection = connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var body = Encoding.UTF8.GetBytes(payload);

                channel.BasicPublish(
                    exchange: queueName,
                    routingKey: "tests",
                    basicProperties: null,
                    body: body);
                return true;
            });
        }
    }
}