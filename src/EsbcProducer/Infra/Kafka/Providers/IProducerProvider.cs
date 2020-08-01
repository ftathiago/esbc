using Confluent.Kafka;

namespace EsbcProducer.Infra.Kafka.Providers
{
    public interface IProducerProvider
    {
        IProducer<Null, string> GetProducer();
    }
}