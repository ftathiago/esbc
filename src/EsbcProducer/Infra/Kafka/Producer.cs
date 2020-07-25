using Confluent.Kafka;
using EsbcProducer.Infra.Kafka.Factories;
using EsbcProducer.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.Kafka
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
            var json = JsonSerializer.Serialize(message, message.GetType());
            _logger.LogInformation($"Producing message: {json}");
            try
            {
                var producer = _producerProvider.GetProducer();
                var result = await producer.ProduceAsync(
                    topicName,
                    new Message<Null, string>
                    {
                        Value = json,
                    },
                    stoppingToken);
                return result.Status == PersistenceStatus.Persisted;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to send message");
                return false;
            }
        }
    }
}