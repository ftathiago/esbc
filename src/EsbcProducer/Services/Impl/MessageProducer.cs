using EsbcProducer.Configurations;
using EsbcProducer.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Services.Impl
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IMessages _messages;
        private readonly MessageConfig _messageConfig;
        private int _messageNumber = 1;

        public MessageProducer(
            IMessages messages,
            IOptionsSnapshot<MessageConfig> messageConfig)
        {
            _messages = messages;
            _messageConfig = messageConfig.Value;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }

            var wait = 5000;
            if (_messageConfig.WaitingTime > 0)
            {
                wait = _messageConfig.WaitingTime;
            }

            await _messages.Send(
                new
                {
                    message = $"{_messageConfig.MessageText} - {_messageNumber} at {DateTimeOffset.Now}",
                });
            _messageNumber++;

            await Task.Delay(wait, stoppingToken);
        }
    }
}