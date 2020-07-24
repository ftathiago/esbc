using Confluent.Kafka;
using EsbcProducer.Services;
using Microsoft.Extensions.Configuration;
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
        private readonly string _bootstrapserver;
        private readonly ProducerConfig _producerConfig;

        public Producer(ILogger<Producer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _bootstrapserver = configuration["Queue:Host"];
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = $"{_bootstrapserver}:9092",
                RequestTimeoutMs = 5000,
            };
        }

        public async Task Send(object message, CancellationToken stoppingToken)
        {
            var json = JsonSerializer.Serialize(message, message.GetType());
            _logger.LogInformation($"Producing message: {json} to server {_bootstrapserver}");
            try
            {
                using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
                await producer.ProduceAsync(
                    "test_topic",
                    new Message<Null, string>
                    {
                        Value = "{ \"message\":\"Teste\"}"
                    },
                    stoppingToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to send message");
            }
        }
    }
}