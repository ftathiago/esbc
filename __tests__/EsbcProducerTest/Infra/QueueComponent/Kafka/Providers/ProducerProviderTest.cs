using EsbcProducer.Infra.QueueComponent.Configurations;
using EsbcProducer.Infra.QueueComponent.Kafka.Providers.Impl;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace EsbcProducerTest.Infra.QueueComponent.Kafka.Providers
{
    public sealed class ProducerProviderTest : IDisposable
    {
        private readonly QueueConfiguration _configuration;

        public ProducerProviderTest()
        {
            _configuration = new QueueConfiguration();
        }

        public void Dispose()
        {
            Mock.VerifyAll();
        }

        [Fact]
        public void ShouldThrowExceptionWhenHostIsEmpty()
        {
            _configuration.HostName = string.Empty;
            var provider = default(ProducerProvider);

            Action act = () => provider = new ProducerProvider(_configuration);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenPortIsEmpty()
        {
            _configuration.HostName = "localhost";
            _configuration.Port = 0;
            var provider = default(ProducerProvider);

            Action act = () => provider = new ProducerProvider(_configuration);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ShouldReturnSameProducerInstance()
        {
            _configuration.HostName = "localhost";
            _configuration.Port = 9092;
            using var provider = new ProducerProvider(_configuration);

            using var instance1 = provider.GetProducer();
            using var instance2 = provider.GetProducer();

            instance1.Should().Be(instance2);
        }
    }
}