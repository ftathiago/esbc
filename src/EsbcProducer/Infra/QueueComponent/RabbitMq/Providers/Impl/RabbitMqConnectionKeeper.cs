using EsbcProducer.Infra.QueueComponent.Configurations;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Providers.Impl
{
    public class RabbitMqConnectionKeeper : IRabbitMqConnectionKeeper, IDisposable
    {
        private readonly IConnectionFactory _factory;
        private readonly ILogger<RabbitMqConnectionKeeper> _logger;
        private readonly Policy _policy;
        private readonly object _lockObject = new object();
        private IConnection _connection;
        private bool disposedValue;

        public RabbitMqConnectionKeeper(
            QueueConfiguration queueConfiguration,
            IConnectionFactory factory,
            ILogger<RabbitMqConnectionKeeper> logger)
        {
            _factory = factory;
            _logger = logger;
            _policy = DefaultPolicyFactory(
                queueConfiguration.RetryCount,
                queueConfiguration.TimeoutMs);
        }

        public bool IsConnected =>
            _connection?.IsOpen == true && !disposedValue;

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            lock (_lockObject)
            {
                if (IsConnected)
                {
                    return true;
                }

                _policy.Execute(() => _connection = _factory.CreateConnection());

                if (!IsConnected)
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    return false;
                }

                WatchConnectionHealth();

                _logger.LogInformation($"New connection to {_connection.Endpoint.HostName}.");

                return true;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException(
                    "There are RabbitMQ connections available to perform this action");
            }

            return _connection.CreateModel();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Tarefa pendente: descartar o estado gerenciado (objetos gerenciados)
                    try
                    {
                        _connection?.Dispose();
                    }
                    catch (IOException ex)
                    {
                        _logger.LogCritical(ex.ToString());
                    }
                }

                // Tarefa pendente: liberar recursos não gerenciados (objetos não gerenciados) e substituir o finalizador
                // Tarefa pendente: definir campos grandes como nulos
                disposedValue = true;
            }
        }

        private Policy DefaultPolicyFactory(int retryCount, int timeoutMs) =>
            Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(
                    retryCount - 1,
                    _ => TimeSpan.FromMilliseconds(timeoutMs),
                    (ex, _) => _logger.LogWarning(ex.ToString()));

        private void WatchConnectionHealth()
        {
            _connection.ConnectionShutdown += (sender, e) =>
            {
                if (disposedValue)
                {
                    return;
                }

                _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");
                TryConnect();
            };

            _connection.CallbackException += (sender, e) =>
            {
                if (disposedValue)
                {
                    return;
                }

                _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");
                TryConnect();
            };

            _connection.ConnectionBlocked += (sender, e) =>
            {
                if (disposedValue)
                {
                    return;
                }

                _logger.LogWarning("A RabbitMQ connection is blocked. Trying to re-connect...");
                TryConnect();
            };
        }
    }
}