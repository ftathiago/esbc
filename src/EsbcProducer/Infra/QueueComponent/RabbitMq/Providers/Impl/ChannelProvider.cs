using RabbitMQ.Client;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Providers.Impl
{
    public class ChannelProvider : IChannelProvider
    {
        private readonly IConnectionProvider _connectionProvider;
        private IModel _channel;
        private bool disposedValue;

        public ChannelProvider(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Tarefa pendente: descartar o estado gerenciado (objetos gerenciados)
                }

                _channel.Dispose();
                disposedValue = true;
            }
        }
    }
}