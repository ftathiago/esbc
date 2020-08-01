using System;
using EsbcProducer.Infra.Configurations;
using RabbitMQ.Client;

namespace EsbcProducer.Infra.RabbitMq.Providers.Impl
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly QueueConfiguration _queueConfiguration;
        private IConnection _connection;

        public ConnectionProvider(QueueConfiguration queueConfiguration)
        {
            _queueConfiguration = queueConfiguration;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IConnection GetConnection()
        {
            if (_connection is null)
            {
                var connectionFactory = GetConnectionFactory();
                _connection = connectionFactory.CreateConnection();
            }

            return _connection;
        }

        private ConnectionFactory GetConnectionFactory() =>
            new ConnectionFactory
            {
                HostName = _queueConfiguration.HostName,
                Port = _queueConfiguration.Port,
                UserName = _queueConfiguration.User,
                Password = _queueConfiguration.Password,
            };
    }
}