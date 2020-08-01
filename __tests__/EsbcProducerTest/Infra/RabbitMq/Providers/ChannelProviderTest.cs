using EsbcProducer.Infra.RabbitMq.Providers;
using EsbcProducer.Infra.RabbitMq.Providers.Impl;
using FluentAssertions;
using Moq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Xunit;

namespace EsbcProducerTest.Infra.RabbitMq.Providers
{
    public class ChannelProviderTest : IDisposable
    {
        private readonly Mock<IConnection> _connection;
        private readonly Mock<IConnectionProvider> _connectionProvider;
        private readonly Mock<IModel> _channel;

        public ChannelProviderTest()
        {
            _connection = new Mock<IConnection>(MockBehavior.Strict);

            _connectionProvider = new Mock<IConnectionProvider>(MockBehavior.Strict);

            _channel = new Mock<IModel>(MockBehavior.Strict);
            _channel
                .Setup(c => c.Dispose())
                .Verifiable();
        }

        public void Dispose()
        {
            _connectionProvider.Verify();
            _connection.Verify();
            _channel.Verify();
        }

        [Fact]
        public void ShouldCreateNewChannel()
        {
            // Given
            _connection
                .Setup(c => c.CreateModel())
                .Returns(_channel.Object)
                .Verifiable();
            _connectionProvider
                .Setup(cp => cp.GetConnection())
                .Returns(_connection.Object)
                .Verifiable();
            using var channelProvider = new ChannelProvider(_connectionProvider.Object);

            // When
            var channel = channelProvider.GetChannel();

            // Then
            channel.Should().NotBeNull();
        }

        [Fact]
        public void ShouldCreateChannelOneTime()
        {
            // Given
            _connection
                .Setup(c => c.CreateModel())
                .Returns(_channel.Object)
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

        [Fact]
        public void ShouldConfigureAChanellOnDeclareQueue()
        {
            // Given
            const string QueueName = "queueName";
            const bool Durable = true;
            const bool Exclusive = true;
            const bool AutoDelete = true;
            const IDictionary<string, object> Arguments = default;
            var queueDeclareOk = new QueueDeclareOk(QueueName, 0, 0);
            _channel
                .Setup(c => c.QueueDeclare(QueueName, Durable, !Exclusive, !AutoDelete, Arguments))
                .Returns(queueDeclareOk)
                .Verifiable();
            _connection
                .Setup(c => c.CreateModel())
                .Returns(_channel.Object)
                .Verifiable();
            _connectionProvider
                .Setup(cp => cp.GetConnection())
                .Returns(_connection.Object)
                .Verifiable();
            using var channelProvider = new ChannelProvider(_connectionProvider.Object);

            // When
            var expectedReturn = channelProvider.QueueDeclare(QueueName);

            // Then
            expectedReturn.Should().Be(channelProvider);
            _connectionProvider.Verify(cp => cp.GetConnection(), Times.Once());
        }
    }
}