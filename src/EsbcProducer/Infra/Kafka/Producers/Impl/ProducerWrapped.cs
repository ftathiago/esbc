using Confluent.Kafka;
using EsbcProducer.Infra.Kafka.Providers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.Kafka.Producers.Impl
{
    public class ProducerWrapped : IKafkaProducerWrapped
    {
        private readonly ILogger<ProducerWrapped> _logger;
        private readonly IProducerProvider _producerProvider;

        public ProducerWrapped(ILogger<ProducerWrapped> logger, IProducerProvider producerProvider)
        {
            _logger = logger;
            _producerProvider = producerProvider;
        }

        public async Task<bool> Send(
            string queueName,
            string payload,
            CancellationToken stoppingToken = default)
        {
            _logger.LogInformation($"Producing message: {payload}");
            var message = GetMessage(payload);
            try
            {
                var producer = _producerProvider.GetProducer();
                var result = await producer.ProduceAsync(
                    queueName,
                    message,
                    stoppingToken);
                return result.Status == PersistenceStatus.Persisted;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to send message");
                return false;
            }
        }

        private Message<Null, string> GetMessage(string payload) =>
            new Message<Null, string>
            {
                Value = payload,
            };
    }
}