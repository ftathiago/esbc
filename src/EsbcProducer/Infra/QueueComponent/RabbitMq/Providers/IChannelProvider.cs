using RabbitMQ.Client;
using System;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Providers
{
    public interface IChannelProvider : IDisposable
    {
        IChannelProvider QueueDeclare(string queueName);

        IModel GetChannel();
    }
}