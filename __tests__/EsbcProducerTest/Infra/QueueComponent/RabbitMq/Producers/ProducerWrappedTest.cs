using EsbcProducer.Fixtures;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Producers.Impl;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Providers;
using FluentAssertions;
using Moq;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EsbcProducerTest.Infra.QueueComponent.RabbitMq.Producers
{
    public sealed class ProducerWrappedTest : IClassFixture<MessageFixture>, IDisposable
    {
        private const string QueueName = "QueueName";
        private readonly Mock<IChannelProvider> _channelProvider;
        private readonly Mock<IModel> _channel;
        private readonly MessageFixture _messageFixture;

        public ProducerWrappedTest(MessageFixture messageFixture)
        {
            _channelProvider = new Mock<IChannelProvider>(MockBehavior.Strict);
            _channel = new Mock<IModel>();
            _messageFixture = messageFixture;
        }

        public void Dispose()
        {
            Mock.VerifyAll();
        }

        [Fact]
        public async Task ShouldSendAMessageAsync()
        {
            // Given
            var token = new CancellationToken(false);
            var payload = _messageFixture.GetMessageString();
            ReadOnlyMemory<byte> payloadBytes = _messageFixture.GetMesageUTF8();
            var basicProperties = new Mock<IBasicProperties>().Object;
            _channel
                .Setup(c => c.QueueDeclare(QueueName, true, false, false, null))
                .Returns(new QueueDeclareOk(QueueName, 0, 0))
                .Verifiable();
            _channelProvider
                .Setup(cp => cp.QueueDeclare(QueueName))
                .Returns(_channelProvider.Object);
            _channelProvider
                .Setup(cp => cp.GetChannel())
                .Returns(_channel.Object);
            var producer = new ProducerWrapped(_channelProvider.Object);

            // When
            var messageWasSent = await producer.Send(QueueName, payload, token);

            // Then
            messageWasSent.Should().BeTrue();
        }
    }
}