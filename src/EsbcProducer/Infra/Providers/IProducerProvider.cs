using EsbcProducer.Infra.Wrappers;

namespace EsbcProducer.Infra.Providers
{
    public interface IProducerProvider
    {
        IProducerWrapped GetProducer(QueueMechanism mechanism);
    }
}