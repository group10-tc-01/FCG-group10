using FCG.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Act
            var balance = Balance.Create(zeroAmount);

            // Assert
            balance.Value.Should().Be(0);
        }

        [Fact]
        public void Given_NegativeAmount_When_CreateBalance_Then_ShouldThrowArgumentException()
        {
            // Arrange
            decimal negativeAmount = -10m;

            // Act
            Action act = () => Balance.Create(negativeAmount);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Balance cannot be negative .");
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
        public void Given_BalanceObject_When_ImplicitConvertToDecimal_Then_ShouldReturnValue()
        {
            // Arrange
            var balance = Balance.Create(50.75m);

            // Act
            decimal value = balance;

            // Assert
            value.Should().Be(50.75m);
        }

        [Fact]
        public void Given_DecimalValue_When_ImplicitConvertToBalance_Then_ShouldCreateBalance()
        {
            // Arrange
            decimal value = 25.99m;

            // Act
            Balance balance = value;

            // Assert
            balance.Value.Should().Be(25.99m);
        }

        [Fact]
        public void Given_TwoBalancesWithSameValue_When_Compare_Then_ShouldBeEqual()
        {
            // Arrange
            var balance1 = Balance.Create(100m);
            var balance2 = Balance.Create(100m);

            // Act & Assert
            balance1.Should().Be(balance2);
            balance1.GetHashCode().Should().Be(balance2.GetHashCode());
        }

        [Fact]
        public void Given_TwoBalancesWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {
            // Arrange
            var balance1 = Balance.Create(100m);
            var balance2 = Balance.Create(200m);

            // Act & Assert
            balance1.Should().NotBe(balance2);
        }
    }
}
