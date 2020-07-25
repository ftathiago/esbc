﻿using System;
using Confluent.Kafka;
using EsbcProducer.Fixtures;
using EsbcProducer.Infra.Kafka;
using EsbcProducer.Infra.Kafka.Factories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EsbcProducerTest.Infra.Kafka
{
    public class ProducerTest : IClassFixture<MessageFixture>, IDisposable
    {
        private const string TopicName = "topic_name";
        private readonly MessageFixture _message;
        private readonly Mock<ILogger<Producer>> _logger;
        private readonly Mock<IProducer<Null, string>> _producer;
        private readonly Mock<IProducerProvider> _producerProvider;

        public ProducerTest(MessageFixture message)
        {
            _message = message;
            _logger = new Mock<ILogger<Producer>>();
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
            var producer = new Producer(_logger.Object, _producerProvider.Object);

            producer.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldReturnFalseWhenMessageIsNotPersisted()
        {
            var objJson = _message.GetMessageString();
            var obj = _message.GetMessageObject();
            var message = new Message<Null, string> { Value = objJson };
            var cancellationToken = default(CancellationToken);
            var sentMessage = default(Message<Null, string>);
            _producerProvider
                .Setup(pp => pp.GetProducer())
                .Returns(_producer.Object)
                .Verifiable();
            _producer
                .Setup(p => p.ProduceAsync(TopicName, It.IsAny<Message<Null, string>>(), cancellationToken))
                .Callback<string, Message<Null, string>, CancellationToken>((tn, message, c) => sentMessage = message)
                .ReturnsAsync(GetDeliveryNotPersisted())
                .Verifiable();
            var producer = new Producer(_logger.Object, _producerProvider.Object);

            var messageSent = await producer.Send(TopicName, obj, CancellationToken.None);

            sentMessage.Value.Should().Be(objJson);
            messageSent.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnTrueWhenMessageIsPersisted()
        {
            var objJson = _message.GetMessageString();
            var obj = _message.GetMessageObject();
            var message = new Message<Null, string> { Value = objJson };
            var cancellationToken = default(CancellationToken);
            var sentMessage = default(Message<Null, string>);
            _producerProvider
                .Setup(pp => pp.GetProducer())
                .Returns(_producer.Object)
                .Verifiable();
            _producer
                .Setup(p => p.ProduceAsync(TopicName, It.IsAny<Message<Null, string>>(), cancellationToken))
                .Callback<string, Message<Null, string>, CancellationToken>((tn, message, c) => sentMessage = message)
                .ReturnsAsync(GetDeliveryPersisted())
                .Verifiable();
            var producer = new Producer(_logger.Object, _producerProvider.Object);

            var messageWasSent = await producer.Send(TopicName, obj, CancellationToken.None);

            sentMessage.Value.Should().Be(objJson);
            messageWasSent.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnFalseWhenThrowsAnyException()
        {
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
            var producer = new Producer(_logger.Object, _producerProvider.Object);

            var messageSent = await producer.Send(TopicName, obj, CancellationToken.None);

            messageSent.Should().BeFalse();
        }

        private DeliveryResult<Null, string> GetDeliveryPersisted() =>
             new DeliveryResult<Null, string> { Status = PersistenceStatus.Persisted };

        private DeliveryResult<Null, string> GetDeliveryNotPersisted() =>
             new DeliveryResult<Null, string> { Status = PersistenceStatus.NotPersisted };
    }
}