using System;
using EsbcProducer.Infra.RabbitMq.Providers;
using EsbcProducer.Infra.RabbitMq.Providers.Impl;
using FluentAssertions;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace EsbcProducerTest.Infra.RabbitMq.Providers
{
    public class ChannelProviderTest : IDisposable
    {
        private readonly Mock<IConnectionProvider> _connectionProvider;
        private readonly Mock<IConnection> _connection;
        private readonly IModel _channel;

        public ChannelProviderTest()
        {
            _connectionProvider = new Mock<IConnectionProvider>(MockBehavior.Strict);
            _connectionProvider
                .Setup(cp => cp.Dispose());
            _connection = new Mock<IConnection>(MockBehavior.Strict);
            _connection
                .Setup(c => c.Dispose());
            var channel = new Mock<IModel>(MockBehavior.Strict);
            channel
                .Setup(c => c.Dispose());
            _channel = channel.Object;
        }

        public void Dispose()
        {
            Mock.VerifyAll();
        }

        [Fact]
        public void ShouldCreateNewChannel()
        {
            // Given
            _connection
                .Setup(c => c.CreateModel())
                .Returns(_channel)
                .Verifiable();
            _connectionProvider
                .Setup(cp => cp.GetConnection())
                .Returns(_connection.Object)
                .Verifiable();
            using var channelProvider = new ChannelProvider(_connectionProvider.Object);

            // When
            using var channel = channelProvider.GetChannel();

            // Then
            channel.Should().NotBeNull();
        }

        [Fact]
        public void ShouldCreateChannelOneTime()
        {
            // Given
            _connection
                .Setup(c => c.CreateModel())
                .Returns(_channel)
                .Verifiable();
            _connectionProvider
                .Setup(cp => cp.GetConnection())
                .Returns(_connection.Object)
                .Verifiable();
            using var channelProvider = new ChannelProvider(_connectionProvider.Object);

            // When
            var channel = channelProvider.GetChannel();
            var chanell2 = channelProvider.GetChannel();

            // Then
            _connection.Verify(c => c.CreateModel(), Times.Once());
            _connectionProvider.Verify(cp => cp.GetConnection(), Times.Once());
            channel.Should().Be(chanell2);
        }
    }
}