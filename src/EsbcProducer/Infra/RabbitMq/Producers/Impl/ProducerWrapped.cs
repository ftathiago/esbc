using EsbcProducer.Infra.RabbitMq.Providers;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.RabbitMq.Producers.Impl
{
    public class ProducerWrapped : IRabbitMqProducerWrapped
    {
        private readonly IChannelProvider _channelProvider;

        public ProducerWrapped(IChannelProvider channelProvider)
        {
            _channelProvider = channelProvider;
        }

        public Task<bool> Send(string queueName, string payload, CancellationToken stoppingToken = default)
        {
            return Task.Run<bool>(() =>
            {
                var body = Encoding.UTF8.GetBytes(payload);

                var channel = _channelProvider.GetChannel();
                channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

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