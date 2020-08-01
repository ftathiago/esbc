using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.Wrappers
{
    public interface IProducerWrapped
    {
        Task<bool> Send(
            string queueName,
            string payload,
            CancellationToken stoppingToken = default);
    }
}