using EsbcProducer.Infra.Providers;
using EsbcProducer.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Infra
{
    public class Producer : IProducer
    {
        private readonly ILogger<Producer> _logger;
        private readonly IProducerProvider _producerProvider;

        public Producer(ILogger<Producer> logger, IProducerProvider producerProvider)
        {
            _logger = logger;
            _producerProvider = producerProvider;
        }

        public async Task<bool> Send(string topicName, object message, CancellationToken stoppingToken)
        {
            var serializedMessage = JsonSerializer.Serialize(message, message.GetType());
            _logger.LogInformation($"Producing message: {serializedMessage}");
            try
            {
                var producer = _producerProvider.GetProducer(QueueMechanism.Kafka);
                return await producer.Send(topicName, serializedMessage, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to send message");
                return false;
            }
        }
    }
}