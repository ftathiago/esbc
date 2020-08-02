using EsbcProducer.Infra.QueueComponent.Configurations;
using RabbitMQ.Client;
using System;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Providers.Impl
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly QueueConfiguration _queueConfiguration;
        private IConnection _connection;
        private bool disposedValue;

        public ConnectionProvider(QueueConfiguration queueConfiguration)
        {
            _queueConfiguration = queueConfiguration;
        }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Tarefa pendente: descartar o estado gerenciado (objetos gerenciados)
                }

                _connection.Dispose();
                disposedValue = true;
            }
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