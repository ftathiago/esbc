using System;
using System.Threading;
using System.Threading.Tasks;
using EsbcProducer.Fixtures;
using EsbcProducer.Infra;
using EsbcProducer.Infra.Configurations;
using EsbcProducer.Infra.Providers;
using EsbcProducer.Infra.Wrappers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EsbcProducerTest.Infra
{
    public class ProducerTest : IClassFixture<MessageFixture>, IDisposable
    {
        private const string TopicName = "topicName";
        private readonly Mock<ILogger<Producer>> _logger;
        private readonly Mock<IProducerProvider> _producerProvider;
        private readonly Mock<IProducerWrapped> _producer;
        private readonly QueueConfiguration _configuration;
        private readonly MessageFixture _messageFixture;

        public ProducerTest(MessageFixture messageFixture)
        {
            _logger = new Mock<ILogger<Producer>>();
            _producerProvider = new Mock<IProducerProvider>(MockBehavior.Strict);
            _producer = new Mock<IProducerWrapped>(MockBehavior.Strict);
            _configuration = new QueueConfiguration();
            _messageFixture = messageFixture;
        }

        public void Dispose()
        {
            Mock.VerifyAll();
        }

        [Fact]
        public async Task ShouldReturnTrueWhenMessageWasSent()
        {
            // Given
            var objJson = _messageFixture.GetMessageString();
            var stoppingToken = new CancellationToken(false);
            var messageObject = _messageFixture.GetMessageObject();
            _producer
                .Setup(p => p.Send(TopicName, objJson, stoppingToken))
                .ReturnsAsync(true);
            _producerProvider
                .Setup(pp => pp.GetProducer(It.IsAny<QueueMechanism>()))
                .Returns(_producer.Object)
                .Verifiable();
            var producer = new Producer(
                _logger.Object,
                _producerProvider.Object,
                _configuration);

            // When
            var messageWasSent = await producer.Send(TopicName, messageObject, stoppingToken);

            // Then
            messageWasSent.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnFalseWhenMessageWasNotSent()
        {
            // Given
            var objJson = _messageFixture.GetMessageString();
            var stoppingToken = new CancellationToken(false);
            var messageObject = _messageFixture.GetMessageObject();
            _producer
                .Setup(p => p.Send(TopicName, objJson, stoppingToken))
                .ReturnsAsync(false);
            _producerProvider
                .Setup(pp => pp.GetProducer(It.IsAny<QueueMechanism>()))
                .Returns(_producer.Object)
                .Verifiable();
            var producer = new Producer(
                _logger.Object,
                _producerProvider.Object,
                _configuration);

            // When
            var messageWasSent = await producer.Send(TopicName, messageObject, stoppingToken);

            // Then
            messageWasSent.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnFalseWhenProducerThrowsAExceptionAsync()
        {
            // Given
            var objJson = _messageFixture.GetMessageString();
            var stoppingToken = new CancellationToken(false);
            var messageObject = _messageFixture.GetMessageObject();
            _producer
                .Setup(p => p.Send(TopicName, objJson, stoppingToken))
                .ThrowsAsync(new Exception())
                .Verifiable();
            var producer = new Producer(
                _logger.Object,
                _producerProvider.Object,
                _configuration);

            // When
            var messageWasSent = await producer.Send(TopicName, messageObject, stoppingToken);

            // Then
            messageWasSent.Should().BeFalse();
        }
    }
}