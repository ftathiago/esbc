using System.Reflection;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EsbcProducer.Infra.Brokers;
using EsbcProducer.Infra.Brokers.Models;
using EsbcProducer.Infra.QueueComponent.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EsbcProducerTest.Infra.MessagesRepository
{
    public class MessagesTest
    {
        private readonly Mock<IProducer> _producer;

        public MessagesTest()
        {
            _producer = new Mock<IProducer>(MockBehavior.Strict);
        }

        [Fact]
        public async Task ShouldAddNewMessage()
        {
            // Given
            var message = new { payload = "Test" };
            var serializedMessage = JsonSerializer.Serialize(message);
            var messageSent = default(Message);
            var stoppingToken = new CancellationToken(false);
            _producer
                .Setup(p => p.Send(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .Callback<string, object, CancellationToken>((s, message, ct) => messageSent = (Message)message)
                .ReturnsAsync(true)
                .Verifiable();
            var messages = new MessageBroker(_producer.Object);

            // When
            await messages.Send(message);

            // Then
            messageSent.Payload.Should().Be(serializedMessage);
            messageSent.Header.Should().NotBeNull();
            messageSent.Header.MessageId.Should().NotBeEmpty();
        }
    }
}