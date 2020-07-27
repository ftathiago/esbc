using RabbitMQ.Client;
using System;

namespace EsbcProducer.Infra.RabbitMq.Providers
{
    public interface IChannelProvider : IDisposable
    {
        IModel GetChannel();
    }
}