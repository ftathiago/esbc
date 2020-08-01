using EsbcProducer.Infra.QueueComponent.Abstractions;
using EsbcProducer.Repositories;
using EsbcProducer.Services.Impl;
using FluentAssertions;
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
        private readonly Mock<IMessages> _messages;

        public MessageProducerTest()
        {
            _messages = new Mock<IMessages>(MockBehavior.Strict);
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
            var messageProducer = new MessageProducer(_messages.Object);

            await messageProducer.DoWork(cancellationToken);
        }

        [Fact]
        public async Task ShouldNotProduceMessageWhenCancellationWasRequestedAsync()
        {
            var cancellationToken = new CancellationToken(Canceled);
            var messageProducer = new MessageProducer(_messages.Object);

            await messageProducer.DoWork(cancellationToken);
        }

        [Fact]
        public void ShouldRethrowExceptions()
        {
            var cancellationToken = new CancellationToken(!Canceled);
            _messages
                .Setup(p => p.Send(It.IsAny<object>()))
                .Verifiable();
            var messageProducer = new MessageProducer(_messages.Object);

            Func<Task> act = async () => await messageProducer.DoWork(cancellationToken);

            act.Should().Throw<Exception>();
        }
    }
}