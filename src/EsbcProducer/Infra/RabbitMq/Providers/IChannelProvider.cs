using RabbitMQ.Client;

namespace EsbcProducer.Infra.RabbitMq.Providers
{
    public interface IChannelProvider
    {
        IModel GetChannel();
    }
}