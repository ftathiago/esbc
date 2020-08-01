using EsbcProducer.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Services.Impl
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IProducer _producer;

        public MessageProducer(IProducer producer)
        {
            _producer = producer;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }

            await _producer.Send(
                "test_topic",
                new
                {
                    message = $"Producing test at {DateTimeOffset.Now}",
                },
                stoppingToken);
        }
    }
}