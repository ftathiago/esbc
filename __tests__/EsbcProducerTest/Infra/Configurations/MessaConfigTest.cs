using EsbcProducer.Configurations;
using FluentAssertions;
using Xunit;

namespace EsbcProducerTest.Infra.Configurations
{
    public class MessaConfigTest
    {
        private const string MessageText = "MessageText";
        private const int MinimumTimeMs = 1;
        private const int DefaultTimeMs = 5000;

        [Fact]
        public void ShouldReturnDefaultValueWhenMessageIsNull()
        {
            // Given
            var messageConfig = new MessageConfig();

            // When
            messageConfig.MessageText = (string)null;

            // Then
            messageConfig.Should().NotBeNull();
        }

        [Fact]
        public void ShouldReturnDefaultValueWhenMessageIsEmpty()
        {
            // Given
            var messageConfig = new MessageConfig();

            // When
            messageConfig.MessageText = string.Empty;

            // Then
            messageConfig.Should().NotBe(string.Empty);
        }

        [Fact]
        public void ShouldReturnConfiguredMessage()
        {
            // Given
            var messageConfig = new MessageConfig();

            // When
            messageConfig.MessageText = MessageText;

            // Then
            messageConfig.Should().NotBe(MessageText);
        }

        [Fact]
        public void ShouldKeepValuesGreatherThanZero()
        {
            // Given
            var messageConfig = new MessageConfig();

            // When
            messageConfig.WaitingTime = MinimumTimeMs;

            // Then
            messageConfig.WaitingTime.Should().Be(MinimumTimeMs);
        }

        [Fact]
        public void ShouldReturnDefaultTimeWhenIsZeroEquals()
        {
            // Given
            var messageConfig = new MessageConfig();

            // When
            messageConfig.WaitingTime = 0;

            // Then
            messageConfig.WaitingTime.Should().Be(DefaultTimeMs);
        }

        [Fact]
        public void ShouldReturnDefaultTimeWhenIsLessThanZero()
        {
            // Given
            var messageConfig = new MessageConfig();

            // When
            messageConfig.WaitingTime = -1;

            // Then
            messageConfig.WaitingTime.Should().Be(DefaultTimeMs);
        }
    }
}