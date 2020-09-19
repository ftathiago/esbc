using EsbcProducer.Infra.QueueComponent.Configurations;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Providers.Impl;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using Xunit;

namespace EsbcProducerTest.Infra.QueueComponent.RabbitMq.Providers
{
    public sealed class RabbitMqConnectionKeeperTest : IDisposable
    {
        private const int MinimumTimeout = 1;
        private readonly QueueConfiguration _queueConfig;
        private readonly Mock<IConnectionFactory> _connectionFactory;
        private readonly Mock<ILogger<RabbitMqConnectionKeeper>> _logger;
        private readonly Mock<IConnection> _connection;
        private readonly Mock<IModel> _model;

        public RabbitMqConnectionKeeperTest()
        {
            _logger = new Mock<ILogger<RabbitMqConnectionKeeper>>();
            _queueConfig = new QueueConfiguration
            {
                TimeoutMs = MinimumTimeout,
            };
            _connectionFactory = new Mock<IConnectionFactory>(MockBehavior.Strict);
            _connection = new Mock<IConnection>(MockBehavior.Strict);
            _model = new Mock<IModel>(MockBehavior.Strict);
        }

        public void Dispose()
        {
            _connection.VerifyAll();
            _connectionFactory.VerifyAll();
        }

        [Fact]
        public void ShouldCreateRabbitMqConnectionKeeper()
        {
            // When
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // Then
            rabbitMqConnectionKeeper.Should().NotBeNull();
        }

        [Fact]
        public void ShouldDestroy()
        {
            // Given
            var endPoint = new AmqpTcpEndpoint(_queueConfig.HostName);
            _connection
                .SetupGet(c => c.IsOpen)
                .Returns(true)
                .Verifiable();
            _connection
                .SetupGet(c => c.Endpoint)
                .Returns(endPoint)
                .Verifiable();
            _connection
                .Setup(c => c.Dispose());
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Returns(_connection.Object)
                .Verifiable();
            using var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            rabbitMqConnectionKeeper.TryConnect();
            var isConnected = rabbitMqConnectionKeeper.TryConnect();
            rabbitMqConnectionKeeper.Dispose();
            rabbitMqConnectionKeeper.TryConnect();

            // Then
            _connectionFactory.Verify(cf => cf.CreateConnection(), Times.Exactly(2));
            isConnected.Should().BeTrue();
        }

        [Fact]
        public void ShouldIsConnectedReturnFalseWhenIsNotConnected()
        {
            // Given
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            var isConnected = rabbitMqConnectionKeeper.IsConnected;

            // Then
            isConnected.Should().BeFalse();
        }

        [Fact]
        public void ShouldConnect()
        {
            // Given
            var endPoint = new AmqpTcpEndpoint(_queueConfig.HostName);
            _connection
                .SetupGet(c => c.IsOpen)
                .Returns(true)
                .Verifiable();
            _connection
                .SetupGet(c => c.Endpoint)
                .Returns(endPoint)
                .Verifiable();
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Returns(_connection.Object)
                .Verifiable();
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            var isConnected = rabbitMqConnectionKeeper.TryConnect();

            // Then
            isConnected.Should().BeTrue();
        }

        [Fact]
        public void ShouldRetryWhenBrokerUnreachableException()
        {
            // Given
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Throws(new BrokerUnreachableException(new Exception("Test")))
                .Verifiable();
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            Func<bool> act = () => rabbitMqConnectionKeeper.TryConnect();

            // Then
            act.Should().ThrowExactly<BrokerUnreachableException>();
            _connectionFactory.Verify(cf => cf.CreateConnection(), Times.Exactly(_queueConfig.RetryCount));
        }

        [Fact]
        public void ShouldRetryWhenSocketException()
        {
            // Given
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Throws<SocketException>()
                .Verifiable();
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            Func<bool> act = () => rabbitMqConnectionKeeper.TryConnect();

            // Then
            act.Should().ThrowExactly<SocketException>();
            _connectionFactory.Verify(cf => cf.CreateConnection(), Times.Exactly(_queueConfig.RetryCount));
        }

        [Fact]
        public void ShouldNotRetryWhenNonExpectedExceptionOccurrs()
        {
            // Given
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Throws<Exception>()
                .Verifiable();
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            Func<bool> act = () => rabbitMqConnectionKeeper.TryConnect();

            // Then
            act.Should().ThrowExactly<Exception>();
            _connectionFactory.Verify(cf => cf.CreateConnection(), Times.Exactly(1));
        }

        [Fact]
        public void ShouldReturnFalseWhenCanNotConnect()
        {
            // Given
            _connection
                .SetupGet(c => c.IsOpen)
                .Returns(false)
                .Verifiable();
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Returns(_connection.Object)
                .Verifiable();
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            var isConnected = rabbitMqConnectionKeeper.TryConnect();

            // Then
            isConnected.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueWhenAlreadyConnected()
        {
            // Given
            var endPoint = new AmqpTcpEndpoint(_queueConfig.HostName);
            _connection
                .SetupGet(c => c.IsOpen)
                .Returns(true)
                .Verifiable();
            _connection
                .SetupGet(c => c.Endpoint)
                .Returns(endPoint)
                .Verifiable();
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Returns(_connection.Object)
                .Verifiable();
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            rabbitMqConnectionKeeper.TryConnect();
            var isConnected = rabbitMqConnectionKeeper.TryConnect();

            // Then
            _connectionFactory.Verify(cf => cf.CreateConnection(), Times.Exactly(1));
            isConnected.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnANewModel()
        {
            // Given
            var model = _model.Object;
            var endPoint = new AmqpTcpEndpoint(_queueConfig.HostName);
            _connection
                .SetupGet(c => c.IsOpen)
                .Returns(true)
                .Verifiable();
            _connection
                .SetupGet(c => c.Endpoint)
                .Returns(endPoint)
                .Verifiable();
            _connection
                .Setup(c => c.CreateModel())
                .Returns(model)
                .Verifiable();
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Returns(_connection.Object)
                .Verifiable();
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            rabbitMqConnectionKeeper.TryConnect();
            var channel = rabbitMqConnectionKeeper.CreateModel();

            // Then
            channel.Should().NotBeNull();
            channel.Should().Be(model);
        }

        [Fact]
        public void ShouldThrowExceptionWhenThereIsNoConnection()
        {
            // Given
            var times = 0;
            var model = _model.Object;
            var endPoint = new AmqpTcpEndpoint(_queueConfig.HostName);
            _connection
                .SetupGet(c => c.IsOpen)
                .Returns(() =>
                {
                    times++;
                    return times == 1;
                })
                .Verifiable();
            _connection
                .SetupGet(c => c.Endpoint)
                .Returns(endPoint)
                .Verifiable();
            _connectionFactory
                .Setup(cf => cf.CreateConnection())
                .Returns(_connection.Object)
                .Verifiable();
            var rabbitMqConnectionKeeper = new RabbitMqConnectionKeeper(
                _queueConfig,
                _connectionFactory.Object,
                _logger.Object);

            // When
            rabbitMqConnectionKeeper.TryConnect();
            Func<IModel> act = () => rabbitMqConnectionKeeper.CreateModel();

            // Then
            act.Should().Throw<InvalidOperationException>();
        }
    }
}