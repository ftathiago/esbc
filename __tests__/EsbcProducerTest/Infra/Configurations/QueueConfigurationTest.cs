using EsbcProducer.Infra;
using EsbcProducer.Infra.Configurations;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Xunit;

namespace EsbcProducerTest.Infra.Configurations
{
    public class QueueConfigurationTest : IDisposable
    {
        private const string DefaultHostName = "localhost";
        private const string BasePath = "QueueConfiguration";
        private const int DefaultPort = 9092;
        private const QueueMechanism DefaultMechanism = QueueMechanism.Kafka;
        private Mock<IConfiguration> _configuration;

        public QueueConfigurationTest()
        {
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);
        }

        public void Dispose()
        {
            _configuration.Verify();
        }

        [Fact]
        public void ShouldHaveDefaultValues()
        {
            // Given

            // When
            var queueConfig = new QueueConfiguration();

            // Then
            queueConfig.HostName.Should().Be(DefaultHostName);
            queueConfig.Port.Should().Be(DefaultPort);
            queueConfig.QueueMechanism.Should().Be(DefaultMechanism);
            queueConfig.KafkaConfiguration.Should().NotBeNull();
        }

        [Fact]
        public void ShouldLoadConfigurations()
        {
            // Given
            const string userName = "username";
            const string password = "password";
            var requestTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
            _configuration
                .Setup(c => c[$"{BasePath}:HostName"])
                .Returns(DefaultHostName)
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:Port"])
                .Returns(DefaultPort.ToString())
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:User"])
                .Returns(userName)
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:Password"])
                .Returns(password)
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:QueueMechanism"])
                .Returns(nameof(QueueMechanism.Kafka))
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:KafkaConfiguration:RequestTimeoutMs"])
                .Returns(requestTimeout.ToString())
                .Verifiable();

            // When
            var queueConfig = QueueConfiguration.From(_configuration.Object);

            // Then
            queueConfig.HostName.Should().Be(DefaultHostName);
            queueConfig.Port.Should().Be(DefaultPort);
            queueConfig.User.Should().Be(userName);
            queueConfig.Password.Should().Be(password);
            queueConfig.QueueMechanism.Should().Be(QueueMechanism.Kafka);
            queueConfig.KafkaConfiguration.RequestTimeoutMs.Should().Be(requestTimeout);
        }

        [Fact]
        public void ShouldLoadConfigurationsWithDefaultValues()
        {
            // Given
            const string userName = null;
            const string password = null;
            var requestTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
            _configuration
                .Setup(c => c[$"{BasePath}:HostName"])
                .Returns((string)null)
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:Port"])
                .Returns((string)null)
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:User"])
                .Returns(userName)
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:Password"])
                .Returns(password)
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:QueueMechanism"])
                .Returns(nameof(QueueMechanism.Kafka))
                .Verifiable();
            _configuration
                .Setup(c => c[$"{BasePath}:KafkaConfiguration:RequestTimeoutMs"])
                .Returns(requestTimeout.ToString())
                .Verifiable();

            // When
            var queueConfig = QueueConfiguration.From(_configuration.Object);

            // Then
            queueConfig.HostName.Should().Be(DefaultHostName);
            queueConfig.Port.Should().Be(DefaultPort);
            queueConfig.User.Should().Be(userName);
            queueConfig.Password.Should().Be(password);
            queueConfig.QueueMechanism.Should().Be(QueueMechanism.Kafka);
            queueConfig.KafkaConfiguration.RequestTimeoutMs.Should().Be(requestTimeout);
        }
    }
}