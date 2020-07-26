using RabbitMQ.Client;

namespace EsbcProducer.Infra.RabbitMq.Providers
{
    public interface IConnectionProvider
    {
        IConnection GetConnection();
    }
}