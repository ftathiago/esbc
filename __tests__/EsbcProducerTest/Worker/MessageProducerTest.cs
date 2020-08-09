using EsbcProducer.Brokers;
using EsbcProducer.Configurations;
using EsbcProducer.Services.Impl;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EsbcProducerTest.Worker
{
    public class MessageProducerTest : IDisposable
    {
        private const bool Canceled = true;
        private readonly Mock<IMessageBroker> _messages;
        private readonly Mock<IOptionsSnapshot<MessageConfig>> _messageConfig;

        public MessageProducerTest()
        {
            _messages = new Mock<IMessageBroker>(MockBehavior.Strict);
            _messageConfig = new Mock<IOptionsSnapshot<MessageConfig>>();
            _messageConfig
                .SetupGet(mc => mc.Value)
                .Returns(new MessageConfig
                {
                    MessageText = "Producing message text by parameter",
                    WaitingTime = 5000,
                });
        }

        public void Dispose()
        {
            _messages.Verify();
        }

        [Fact]
        public async Task ShouldProduceMessageAsync()
        {
            var cancellationToken = new CancellationToken(!Canceled);
            _messages
                .Setup(p => p.Send(It.IsAny<object>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var messageProducer = new MessageProducer(_messages.Object, _messageConfig.Object);

            await messageProducer.ProduceMessages(cancellationToken);
        }

        [Fact]
        public async Task ShouldNotProduceMessageWhenCancellationWasRequestedAsync()
        {
            var cancellationToken = new CancellationToken(Canceled);
            var messageProducer = new MessageProducer(_messages.Object, _messageConfig.Object);

            await messageProducer.ProduceMessages(cancellationToken);
        }

        [Fact]
        public void ShouldRethrowExceptions()
        {
            var cancellationToken = new CancellationToken(!Canceled);
            _messages
                .Setup(p => p.Send(It.IsAny<object>()))
                .Verifiable();
            var messageProducer = new MessageProducer(_messages.Object, _messageConfig.Object);

            Func<Task> act = async () => await messageProducer.ProduceMessages(cancellationToken);

            act.Should().Throw<Exception>();
        }
    }
}