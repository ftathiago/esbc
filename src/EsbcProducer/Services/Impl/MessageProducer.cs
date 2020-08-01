using EsbcProducer.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Services.Impl
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IMessages _messages;

        public MessageProducer(IMessages messages)
        {
            _messages = messages;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }

            await _messages.Send(
                new
                {
                    message = $"Producing test at {DateTimeOffset.Now}",
                });
        }
    }
}