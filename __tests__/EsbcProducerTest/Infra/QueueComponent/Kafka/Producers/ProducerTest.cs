using Confluent.Kafka;
using EsbcProducer.Fixtures;
using EsbcProducer.Infra.QueueComponent.Kafka.Producers.Impl;
using EsbcProducer.Infra.QueueComponent.Kafka.Providers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EsbcProducerTest.Infra.QueueComponent.Kafka.Producers
{
    public sealed class ProducerTest : IClassFixture<MessageFixture>, IDisposable
    {
        private const string TopicName = "topic_name";
        private readonly MessageFixture _message;
        private readonly Mock<ILogger<ProducerWrapped>> _logger;
        private readonly Mock<IProducer<Null, string>> _producer;
        private readonly Mock<IProducerProvider> _producerProvider;

        public ProducerTest(MessageFixture message)
        {
            _message = message;
            _logger = new Mock<ILogger<ProducerWrapped>>();
            _producer = new Mock<IProducer<Null, string>>(MockBehavior.Strict);
            _producerProvider = new Mock<IProducerProvider>(MockBehavior.Strict);
        }

        public void Dispose()
        {
            _producer.Verify();
            _producerProvider.Verify();
        }

        [Fact]
        public void ShouldCreateProducer()
        {
            var producer = new ProducerWrapped(_logger.Object, _producerProvider.Object);

            producer.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldReturnFalseWhenMessageIsNotPersisted()
        {
            var objJson = _message.GetMessageString();
            var message = new Message<Null, string> { Value = objJson };
            var cancellationToken = default(CancellationToken);
            var sentMessage = default(Message<Null, string>);
            _producerProvider
                .Setup(pp => pp.GetProducer())
                .Returns(_producer.Object)
                .Verifiable();
            _producer
                .Setup(p => p.ProduceAsync(TopicName, It.IsAny<Message<Null, string>>(), cancellationToken))
                .Callback<string, Message<Null, string>, CancellationToken>((_, message, c) => sentMessage = message)
                .ReturnsAsync(GetDeliveryNotPersisted())
                .Verifiable();
            var producer = new ProducerWrapped(_logger.Object, _producerProvider.Object);

            var messageSent = await producer.Send(TopicName, objJson, CancellationToken.None);

            sentMessage.Value.Should().Be(objJson);
            messageSent.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnTrueWhenMessageIsPersisted()
        {
            var objJson = _message.GetMessageString();
            var message = new Message<Null, string> { Value = objJson };
            var cancellationToken = default(CancellationToken);
            var sentMessage = default(Message<Null, string>);
            _producerProvider
                .Setup(pp => pp.GetProducer())
                .Returns(_producer.Object)
                .Verifiable();
            _producer
                .Setup(p => p.ProduceAsync(TopicName, It.IsAny<Message<Null, string>>(), cancellationToken))
                .Callback<string, Message<Null, string>, CancellationToken>((_, message, c) => sentMessage = message)
                .ReturnsAsync(GetDeliveryPersisted())
                .Verifiable();
            var producer = new ProducerWrapped(_logger.Object, _producerProvider.Object);

            var messageWasSent = await producer.Send(TopicName, objJson, CancellationToken.None);

            sentMessage.Value.Should().Be(objJson);
            messageWasSent.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnFalseWhenThrowsAnyException()
        {
            var objJson = _message.GetMessageString();
            var obj = _message.GetMessageObject();
            var cancellationToken = default(CancellationToken);
            _producerProvider
                .Setup(pp => pp.GetProducer())
                .Returns(_producer.Object)
                .Verifiable();
            _producer
                .Setup(p => p.ProduceAsync(TopicName, It.IsAny<Message<Null, string>>(), cancellationToken))
                .ThrowsAsync(new Exception())
                .Verifiable();
            var producer = new ProducerWrapped(_logger.Object, _producerProvider.Object);

            var messageSent = await producer.Send(TopicName, objJson, CancellationToken.None);

            messageSent.Should().BeFalse();
        }

        private DeliveryResult<Null, string> GetDeliveryPersisted() =>
             new DeliveryResult<Null, string> { Status = PersistenceStatus.Persisted };

        private DeliveryResult<Null, string> GetDeliveryNotPersisted() =>
             new DeliveryResult<Null, string> { Status = PersistenceStatus.NotPersisted };
    }
}