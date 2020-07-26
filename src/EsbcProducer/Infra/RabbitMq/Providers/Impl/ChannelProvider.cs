using System;
using RabbitMQ.Client;

namespace EsbcProducer.Infra.RabbitMq.Providers.Impl
{
    public class ChannelProvider : IChannelProvider, IDisposable
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