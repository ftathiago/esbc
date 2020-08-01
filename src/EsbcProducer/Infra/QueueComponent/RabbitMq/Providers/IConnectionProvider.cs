using RabbitMQ.Client;
using System;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Providers
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}