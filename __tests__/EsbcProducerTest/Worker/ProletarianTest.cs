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
    public class ProletarianTest : IDisposable
    {
        private const bool Canceled = true;
        private readonly Mock<IProducer> _producer;

        public ProletarianTest()
        {
            _producer = new Mock<IProducer>(MockBehavior.Strict);
        }

        public void Dispose()
        {
            _producer.Verify();
        }

        [Fact]
        public async Task ShouldProduceMessageAsync()
        {
            var cancellationToken = new CancellationToken(!Canceled);
            _producer
                .Setup(p => p.Send(It.IsAny<string>(), It.IsAny<object>(), cancellationToken))
                .ReturnsAsync(true)
                .Verifiable();
            var messageProducer = new MessageProducer(_producer.Object);

            await messageProducer.DoWork(cancellationToken);
        }

        [Fact]
        public async Task ShouldNotProduceMessageWhenCancellationWasRequestedAsync()
        {
            var cancellationToken = new CancellationToken(Canceled);
            var messageProducer = new MessageProducer(_producer.Object);

            await messageProducer.DoWork(cancellationToken);
        }

        [Fact]
        public void ShouldRethrowExceptions()
        {
            var cancellationToken = new CancellationToken(!Canceled);
            _producer
                .Setup(p => p.Send(It.IsAny<string>(), It.IsAny<object>(), cancellationToken))
                .ThrowsAsync(new Exception())
                .Verifiable();
            var messageProducer = new MessageProducer(_producer.Object);

            Func<Task> act = async () => await messageProducer.DoWork(cancellationToken);

            act.Should().Throw<Exception>();
        }
    }
}