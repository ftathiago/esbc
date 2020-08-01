using System;
using RabbitMQ.Client;

namespace EsbcProducer.Infra.RabbitMq.Providers.Impl
{
    public class ChannelProvider : IChannelProvider
    {
        private readonly IConnectionProvider _connectionProvider;
        private IModel _channel;

        public ChannelProvider(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void Dispose()
        {
            _channel.Dispose();
        }

        public IChannelProvider QueueDeclare(string queueName)
        {
            var channel = GetChannel();
            channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            return this;
        }

        public IModel GetChannel()
        {
            if (_channel is null)
            {
                var connection = _connectionProvider.GetConnection();
                _channel = connection.CreateModel();
            }

            return _channel;
        }
    }
}