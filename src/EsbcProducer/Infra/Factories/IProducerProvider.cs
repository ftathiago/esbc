using Confluent.Kafka;

namespace EsbcProducer.Infra.Kafka.Factories
{
    public interface IProducerProvider
    {
        IProducer<Null, string> GetProducer();
    }
}