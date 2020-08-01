using EsbcProducer.Infra.QueueComponent.Configurations;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Xunit;

namespace EsbcProducerTest.Infra.QueueComponent.Configurations
{
    public class KafkaConfigurationTest : IDisposable
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly int _requestTimeoutMsDefault;

        public KafkaConfigurationTest()
        {
            _configuration = new Mock<IConfiguration>();
            _requestTimeoutMsDefault = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
        }

        public void Dispose()
        {
            _configuration.VerifyAll();
        }

        [Fact]
        public void ShouldHaveDefaultValues()
        {
            var kafkaConfig = new KafkaConfiguration();

            kafkaConfig.RequestTimeoutMs.Should().Be(_requestTimeoutMsDefault);
        }

        [Fact]
        public void ShouldReadValuesFromConfiguration()
        {
            // Given
            _configuration
                .Setup(c => c["QueueConfiguration:KafkaConfiguration:RequestTimeoutMs"])
                .Returns(_requestTimeoutMsDefault.ToString());

            // When
            var kafkaConfiguration = KafkaConfiguration.From(_configuration.Object);

            // Then
            kafkaConfiguration.RequestTimeoutMs.Should().Be(_requestTimeoutMsDefault);
        }
    }
}