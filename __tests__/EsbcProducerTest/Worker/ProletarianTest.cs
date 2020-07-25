using EsbcProducer.Repositories;
using EsbcProducer.Worker.Impl;
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
            var proletarian = new Proletarian(_producer.Object);

            await proletarian.DoWork(cancellationToken);
        }

        [Fact]
        public async Task ShouldNotProduceMessageWhenCancellationWasRequestedAsync()
        {
            var cancellationToken = new CancellationToken(Canceled);
            var proletarian = new Proletarian(_producer.Object);

            await proletarian.DoWork(cancellationToken);
        }

        [Fact]
        public void ShouldRethrowExceptions()
        {
            var cancellationToken = new CancellationToken(!Canceled);
            _producer
                .Setup(p => p.Send(It.IsAny<string>(), It.IsAny<object>(), cancellationToken))
                .ThrowsAsync(new Exception())
                .Verifiable();
            var proletarian = new Proletarian(_producer.Object);

            Func<Task> act = async () => await proletarian.DoWork(cancellationToken);

            act.Should().Throw<Exception>();
        }
    }
}