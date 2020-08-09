using EsbcProducer.Brokers;
using EsbcProducer.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Services.Impl
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IMessageBroker _broker;
        private readonly MessageConfig _messageConfig;
        private int _messageNumber = 1;

        public MessageProducer(
            IMessageBroker broker,
            IOptionsSnapshot<MessageConfig> messageConfig)
        {
            _broker = broker;
            _messageConfig = messageConfig.Value;
        }

        public async Task ProduceMessages(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }

            var wait = GetWaitingTime();

            await _broker.Send(
                new
                {
                    message = $"{_messageConfig.MessageText} - {_messageNumber} at {DateTimeOffset.Now}",
                });
            _messageNumber++;

            await Task.Delay(wait, stoppingToken);
        }

        private int GetWaitingTime() =>
            _messageConfig.WaitingTime > 0
                ? _messageConfig.WaitingTime
                : 5000;
    }
}