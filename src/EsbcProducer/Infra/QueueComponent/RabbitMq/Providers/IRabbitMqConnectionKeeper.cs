using RabbitMQ.Client;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Providers
{
    public interface IRabbitMqConnectionKeeper
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}