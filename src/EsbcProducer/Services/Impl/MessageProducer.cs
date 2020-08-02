using EsbcProducer.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Services.Impl
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IMessages _messages;
        private int _messageNumber = 1;

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
                    message = $"Producing {_messageNumber} at {DateTimeOffset.Now}",
                });
            _messageNumber++;
        }
    }
}