using EsbcProducer.Infra.Kafka.Producers;
using EsbcProducer.Infra.Wrappers;
using System;
using System.Collections.Generic;

namespace EsbcProducer.Infra.Providers.Impl
{
    public class ProducerProvider : IProducerProvider
    {
        private readonly Dictionary<QueueMechanism, Lazy<IKafkaProducerWrapped>> _producers;

        public ProducerProvider(
            Lazy<IKafkaProducerWrapped> kafkaProvider)
        {
            _producers = new Dictionary<QueueMechanism, Lazy<IKafkaProducerWrapped>>
            {
                { QueueMechanism.Kafka, kafkaProvider },
            };
        }

        public IProducerWrapped GetProducer(QueueMechanism mechanism)
        {
            var producerWasFound = _producers.TryGetValue(mechanism, out var producer);
            if (!producerWasFound)
            {
                throw new ArgumentException($"There is no queue provider for {mechanism}.");
            }

            return producer.Value;
        }
    }
}