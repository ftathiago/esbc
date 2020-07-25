using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Worker
{
    public interface IProletarian
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}