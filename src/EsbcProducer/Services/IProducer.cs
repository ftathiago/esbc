using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Services
{
    public interface IProducer
    {
        Task Send(object message, CancellationToken stoppingToken);
    }
}