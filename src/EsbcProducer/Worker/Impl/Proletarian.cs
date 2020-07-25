using EsbcProducer.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Worker.Impl
{
    public class Proletarian : IProletarian
    {
        private readonly IProducer _producer;

        public Proletarian(IProducer producer)
        {
            _producer = producer;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }

            await _producer
                .Send(
                    "test_topic",
                    new
                    {
                        message = $"Producing test at {DateTimeOffset.Now}",
                    },
                    stoppingToken);
        }
    }
}