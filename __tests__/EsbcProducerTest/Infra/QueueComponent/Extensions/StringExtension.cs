using System;
using EsbcProducer.Infra.QueueComponent.Extensions;
using EsbcProducerTest.Fixtures;
using FluentAssertions;
using Xunit;

namespace EsbcProducerTest.Infra.QueueComponent.Extensions
{
    public class StringExtension
    {
        private const int ExpectedNumber = 20;

        [Fact]
        public void ShouldParseToEnum()
        {
            // Given
            const string element1 = "Element1";

            // When
            var parsedEnum = element1.ParseToEnum<EnumTestFixture>();

            // Then
            parsedEnum.Should().Be(EnumTestFixture.Element1);
        }

        [Fact]
        public void ShouldThrowExceptionWhenTryParseIsInvalid()
        {
            // Given
            const string element1 = "InvalidEnum String";

            // When
            Func<EnumTestFixture> act = () => element1.ParseToEnum<EnumTestFixture>();

            // Then
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldParseToInt()
        {
            // Given
            var numberStr = ExpectedNumber.ToString();

            // When
            var parsedInt = numberStr.ParseToInt();

            // Then
            parsedInt.Should().Be(ExpectedNumber);
        }

        [Fact]
        public void ShouldReturnDefaultNumberWhenStringIsEmpty()
        {
            // Given
            var numberStr = string.Empty;

            // When
            var parsedInt = numberStr.ParseToInt(ExpectedNumber);

            // Then
            parsedInt.Should().Be(ExpectedNumber);
        }

        [Fact]
        public void ShouldReturnDefaultNumberWhenStringIsNull()
        {
            // Given
            const string numberStr = null;

            // When
            var parsedInt = numberStr.ParseToInt(ExpectedNumber);

            // Then
            parsedInt.Should().Be(ExpectedNumber);
        }

        [Fact]
        public void ShouldReturnDefaultNumberWhenStringIsNotANumber()
        {
            // Given
            const string numberStr = "this is not a number";

            // When
            var parsedInt = numberStr.ParseToInt(ExpectedNumber);

            // Then
            parsedInt.Should().Be(ExpectedNumber);
        }
    }
}