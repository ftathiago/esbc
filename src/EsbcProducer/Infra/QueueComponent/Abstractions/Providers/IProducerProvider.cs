using EsbcProducer.Infra.QueueComponent.Abstractions.Wrappers;

namespace EsbcProducer.Infra.QueueComponent.Abstractions.Providers
{
    public interface IProducerProvider
    {
        IProducerWrapped GetProducer(QueueMechanism mechanism);
    }
}