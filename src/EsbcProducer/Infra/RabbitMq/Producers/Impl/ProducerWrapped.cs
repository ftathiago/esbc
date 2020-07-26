using EsbcProducer.Infra.Configurations;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.RabbitMq.Producers.Impl
{
    public class ProducerWrapped : IRabbitMqProducerWrapped
    {
        private readonly QueueConfiguration _queueConfiguration;

        public ProducerWrapped(QueueConfiguration queueConfiguration)
        {
            _queueConfiguration = queueConfiguration;
        }

        public Task<bool> Send(string queueName, string payload, CancellationToken stoppingToken = default)
        {
            return Task.Run<bool>(() =>
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = _queueConfiguration.HostName,
                    Port = _queueConfiguration.Port,
                    UserName = _queueConfiguration.User,
                    Password = _queueConfiguration.Password,
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