using EsbcProducer.Infra;
using EsbcProducer.Infra.Kafka.Producers;
using EsbcProducer.Infra.Providers.Impl;
using EsbcProducer.Infra.RabbitMq.Producers;
using EsbcProducer.Infra.Wrappers;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace EsbcProducerTest.Infra.Providers
{
    public class ProducerProviderTest : IDisposable
    {
        private readonly IKafkaProducerWrapped _kafkaProducerStub;
        private readonly Lazy<IKafkaProducerWrapped> _kafkaProducerLazy;
        private readonly IRabbitMqProducerWrapped _rabbitProducerStub;
        private readonly Lazy<IRabbitMqProducerWrapped> _rabbitProducerLazy;

        public ProducerProviderTest()
        {
            _kafkaProducerStub = new Mock<IKafkaProducerWrapped>(MockBehavior.Strict).Object;
            _kafkaProducerLazy = new Lazy<IKafkaProducerWrapped>(() => _kafkaProducerStub);

            _rabbitProducerStub = new Mock<IRabbitMqProducerWrapped>(MockBehavior.Strict).Object;
            _rabbitProducerLazy = new Lazy<IRabbitMqProducerWrapped>(() => _rabbitProducerStub);
        }

        public void Dispose()
        {
            Mock.VerifyAll();
        }

        [Fact]
        public void ShouldThrowExceptionWhenMechanismIsUnknown()
        {
            // Given
            var producerProvider = CreateProducerProvider();

            // When
            Func<IProducerWrapped> act = () => producerProvider.GetProducer(QueueMechanism.Unknown);

            // Then
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldReturnKafkaWrappedProducer()
        {
            // Given
            var producerProvider = CreateProducerProvider();

            // When
            var producer = producerProvider.GetProducer(QueueMechanism.Kafka);

            // Then
            producer.Should().Be(_kafkaProducerStub);
        }

        [Fact]
        public void ShouldReturnRabbitMqWrappedProducer()
        {
            // Given
            var producerProvider = CreateProducerProvider();

            // When
            var producer = producerProvider.GetProducer(QueueMechanism.RabbitMq);

            // Then
            producer.Should().Be(_rabbitProducerStub);
        }

        private ProducerProvider CreateProducerProvider() =>
            new ProducerProvider(
                _kafkaProducerLazy,
                _rabbitProducerLazy);
    }
}