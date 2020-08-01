using RabbitMQ.Client;
using System;

namespace EsbcProducer.Infra.RabbitMq.Providers
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}