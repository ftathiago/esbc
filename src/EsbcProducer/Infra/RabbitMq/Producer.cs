using System.Threading;
using System.Threading.Tasks;
using EsbcProducer.Infra.Wrappers;

namespace EsbcProducer.Infra.RabbitMq
{
    public class Producer : IProducerWrapped
    {
        public Task<bool> Send(
            string queueName,
            string payload,
            CancellationToken stoppingToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}