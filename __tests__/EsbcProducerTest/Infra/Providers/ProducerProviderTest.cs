using System;
using EsbcProducer.Infra;
using EsbcProducer.Infra.Kafka.Producers;
using EsbcProducer.Infra.Providers.Impl;
using EsbcProducer.Infra.Wrappers;
using FluentAssertions;
using Moq;
using Xunit;

namespace EsbcProducerTest.Infra.Providers
{
    public class ProducerProviderTest : IDisposable
    {
        private readonly IKafkaProducerWrapped _kafkaProducerStub;
        private readonly Lazy<IKafkaProducerWrapped> _kafkaProducerLazy;

        public ProducerProviderTest()
        {
            _kafkaProducerStub = new Mock<IKafkaProducerWrapped>(MockBehavior.Strict).Object;
            _kafkaProducerLazy = new Lazy<IKafkaProducerWrapped>(() => _kafkaProducerStub);
        }

        public void Dispose()
        {
            Mock.VerifyAll();
        }

        [Fact]
        public void ShouldThrowExceptionWhenMechanismNotFound()
        {
            // Given
            const QueueMechanism unknownQueueMechanism = (QueueMechanism)10;
            var producerProvider = new ProducerProvider(_kafkaProducerLazy);

            // When
            Func<IProducerWrapped> act = () => producerProvider.GetProducer(unknownQueueMechanism);

            // Then
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldReturnKafkaWrappedProducer()
        {
            // Given
            var producerProvider = new ProducerProvider(_kafkaProducerLazy);

            // When
            var producer = producerProvider.GetProducer(QueueMechanism.Kafka);

            // Then
            producer.Should().Be(_kafkaProducerStub);
        }
    }
}