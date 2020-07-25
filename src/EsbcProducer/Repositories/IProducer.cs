using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Repositories
{
    public interface IProducer
    {
        Task<bool> Send(string topicName, object message, CancellationToken stoppingToken);
    }
}