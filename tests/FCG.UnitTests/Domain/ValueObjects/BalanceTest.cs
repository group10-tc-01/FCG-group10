using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentAssertions;
using System.Globalization;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class BalanceTests
    {
        [Fact]
        public void Given_ValidPositiveAmount_When_CreateBalance_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            decimal validAmount = 100.50m;

            // Act
            var balance = Balance.Create(validAmount);

            // Assert
            balance.Should().NotBeNull();
            balance.Value.Should().Be(validAmount);
        }

        [Fact]
        public void Given_ZeroAmount_When_CreateBalance_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            decimal zeroAmount = 0m;

            //Act
            var balance = Balance.Create(zeroAmount);

            // Assert
            balance.Value.Should().Be(0);
        }

        [Fact]
        public void Given_NegativeAmount_When_CreateBalance_Then_ShouldThrowDomainException()
        {
            // Arrange
            decimal negativeAmount = -10m;

            // Act
            Action act = () => Balance.Create(negativeAmount);

            // Assert
            act.Should().Throw<DomainException>().WithMessage(ResourceMessages.BalanceCannotBeNegative);
        }

        [Fact]
        public void Given_LargeAmount_When_CreateBalance_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            decimal largeAmount = decimal.MaxValue;

            // Act
            var balance = Balance.Create(largeAmount);

            // Assert
            balance.Value.Should().Be(decimal.MaxValue);
        }

        [Fact]
        public void Given_BalanceObject_When_CalledToString_Then_ShouldReturnStringValue()
        {
            // Arrange
            decimal amount = 77.77m;
            var balance = Balance.Create(amount);

            // Act
            var resultString = balance.Value.ToString(CultureInfo.InvariantCulture);
            var hashCode = balance.GetHashCode();

            // Assert
            resultString.Should().Be("77.77");
            hashCode.Should().NotBe(0);
        }
    }
}
