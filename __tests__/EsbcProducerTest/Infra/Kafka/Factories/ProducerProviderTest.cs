using System;
using EsbcProducer.Infra.Kafka.Factories.Impl;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace EsbcProducerTest.Infra.Kafka.Factories
{
    public class ProducerProviderTest : IDisposable
    {
        private readonly Mock<IConfiguration> _configuration;

        public ProducerProviderTest()
        {
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);
        }

        public void Dispose()
        {
            _configuration.Verify();
        }

        [Fact]
        public void ShouldThrowsWhenConfigurationIsInvalid()
        {
            _configuration
                    .Setup(c => c["Queue:Host"])
                    .Returns(string.Empty);
            var provider = default(ProducerProvider);

            Action act = () => provider = new ProducerProvider(_configuration.Object);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldReturnSameProducerInstance()
        {
            _configuration
                .Setup(c => c["Queue:Host"])
                .Returns("localhost");
            using var provider = new ProducerProvider(_configuration.Object);

            using var instance1 = provider.GetProducer();
            using var instance2 = provider.GetProducer();

            instance1.Should().Be(instance2);
        }
    }
}