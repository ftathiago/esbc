using System.Threading.Tasks;

namespace EsbcProducer.Brokers
{
    public interface IMessageBroker
    {
        Task Send(object payload);
    }
}