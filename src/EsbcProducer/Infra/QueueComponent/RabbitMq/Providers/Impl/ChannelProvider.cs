using EsbcProducer.Infra.QueueComponent.RabbitMq.Exceptions;
using RabbitMQ.Client;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Providers.Impl
{
    public class ChannelProvider : IChannelProvider
    {
        private readonly IRabbitMqConnectionKeeper _persisterConnection;
        private IModel _channel;
        private bool disposedValue;

        public ChannelProvider(IRabbitMqConnectionKeeper persisterConnection)
        {
            _persisterConnection = persisterConnection;
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
                var connected = _persisterConnection.TryConnect();
                if (!connected)
                {
                    throw new RabbitMqException("Não foi possível estabelecer conexão com o RabbitMQ");
                }

                _channel = _persisterConnection.CreateModel();
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

                _channel?.Dispose();
                disposedValue = true;
            }
        }
    }
}