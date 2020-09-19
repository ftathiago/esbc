using EsbcProducer.Infra.QueueComponent.Abstractions;
using EsbcProducer.Infra.QueueComponent.Abstractions.Providers.Impl;
using EsbcProducer.Infra.QueueComponent.Abstractions.Wrappers;
using EsbcProducer.Infra.QueueComponent.Kafka.Producers;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Producers;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace EsbcProducerTest.Infra.QueueComponent.Providers
{
    public sealed class ProducerProviderTest : IDisposable
    {
        private readonly IKafkaProducerWrapped _kafkaProducerStub;
        private readonly IRabbitMqProducerWrapped _rabbitProducerStub;

        public ProducerProviderTest()
        {
            _kafkaProducerStub = new Mock<IKafkaProducerWrapped>(MockBehavior.Strict).Object;
            _rabbitProducerStub = new Mock<IRabbitMqProducerWrapped>(MockBehavior.Strict).Object;
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
                _kafkaProducerStub,
                _rabbitProducerStub);
    }
}