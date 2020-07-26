using EsbcProducer.Infra.Kafka.Producers;
using EsbcProducer.Infra.RabbitMq.Producers;
using EsbcProducer.Infra.Wrappers;
using System;
using System.Collections.Generic;

namespace EsbcProducer.Infra.Providers.Impl
{
    public class ProducerProvider : IProducerProvider
    {
        private readonly Dictionary<QueueMechanism, Func<IProducerWrapped>> _producers;

        public ProducerProvider(
            IKafkaProducerWrapped kafkaProvider,
            IRabbitMqProducerWrapped rabbitMqProvider)
        {
            _producers = new Dictionary<QueueMechanism, Func<IProducerWrapped>>
            {
                { QueueMechanism.Kafka, () => kafkaProvider },
                { QueueMechanism.RabbitMq, () => rabbitMqProvider },
            };
        }

        public IProducerWrapped GetProducer(QueueMechanism mechanism)
        {
            var producerWasFound = _producers.TryGetValue(mechanism, out var getProducer);
            if (!producerWasFound)
            {
                throw new ArgumentException($"There is no queue provider for {mechanism}.");
            }

            return getProducer();
        }
    }
}