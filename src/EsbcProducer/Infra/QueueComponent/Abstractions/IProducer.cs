using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.QueueComponent.Abstractions
{
    public interface IProducer
    {
        Task<bool> Send(string topicName, object message, CancellationToken stoppingToken = default);
    }
}