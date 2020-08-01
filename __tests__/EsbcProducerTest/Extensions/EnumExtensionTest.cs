using System;
using EsbcProducer.Extensions;
using EsbcProducerTest.Fixtures;
using FluentAssertions;
using Xunit;

namespace EsbcProducerTest.Extensions
{
    public class EnumExtensionTest
    {
        [Fact]
        public void ShouldParseEnum()
        {
            // Given
            const string enumTest = "Element1";

            // When
            var enumParsed = enumTest.Parse<EnumTestFixture>();

            // Then
            enumParsed.Should().Be(EnumTestFixture.Element1);
        }

        [Fact]
        public void ShouldThrowExceptionWhenStringIsNotValid()
        {
            // Given
            const string invalidElement = "InvalidElement";

            // When
            Func<EnumTestFixture> act = () => invalidElement.Parse<EnumTestFixture>();

            // Then
            act.Should().ThrowExactly<ArgumentException>();
        }
    }
}