using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Services
{
    public interface IMessageProducer
    {
        Task ProduceMessages(CancellationToken stoppingToken);
    }
}