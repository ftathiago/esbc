using Confluent.Kafka;

namespace EsbcProducer.Infra.QueueComponent.Kafka.Providers
{
    public interface IProducerProvider
    {
        IProducer<Null, string> GetProducer();
    }
}