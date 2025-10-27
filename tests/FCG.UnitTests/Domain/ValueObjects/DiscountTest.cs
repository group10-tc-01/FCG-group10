using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentAssertions;
using System.Globalization;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class DiscountTests
    {
        [Fact]
        public void Given_ValidPercentages_When_CreateDiscount_Then_ShouldCreateSuccessfullyAndSetProperties()
        {
            // Arrange
            decimal validPercentage = 25.5m;
            decimal zeroPercentage = 0m;
            decimal maxPercentage = 100m;

            // Act
            var discount = Discount.Create(validPercentage);
            var zeroDiscount = Discount.Create(zeroPercentage);
            var maxDiscount = Discount.Create(maxPercentage);

            // Assert
            discount.Should().NotBeNull();
            discount.Value.Should().Be(validPercentage);
            zeroDiscount.Value.Should().Be(0);
            maxDiscount.Value.Should().Be(100);
        }

        [Fact]
        public void Given_InvalidPercentages_When_CreateDiscount_Then_ShouldThrowDomainException()
        {
            // Arrange
            decimal negativePercentage = -5m;
            decimal invalidPercentage = 101m;

            // Act
            Action actNegative = () => Discount.Create(negativePercentage);
            Action actAbove100 = () => Discount.Create(invalidPercentage);

            // Assert
            actNegative.Should().Throw<DomainException>().WithMessage(ResourceMessages.DiscountMustBeBetweenZeroAndHundred);
            actAbove100.Should().Throw<DomainException>().WithMessage(ResourceMessages.DiscountMustBeBetweenZeroAndHundred);
        }

        [Fact]
        public void Given_DiscountObject_When_ImplicitlyConvertedToDecimal_Then_ShouldReturnValue()
        {
            // Arrange
            var discount = Discount.Create(15.75m);

            // Act
            decimal value = discount;

            // Assert
            value.Should().Be(15.75m);
        }

        [Fact]
        public void Given_DecimalValue_When_ImplicitlyConvertedToDiscount_Then_ShouldCreateDiscount()
        {
            // Arrange
            decimal value = 30m;

            // Act
            Discount discount = value;

            // Assert
            discount.Value.Should().Be(30m);
        }

        [Fact]
        public void Given_DiscountObject_When_CallToString_Then_ShouldReturnFormattedValue()
        {
            // Arrange
            var discount1 = Discount.Create(15.5m);
            var discount2 = Discount.Create(15m);
            var discount3 = Discount.Create(15.555m);

            // Act
            var result1 = discount1.Value.ToString("F2", CultureInfo.InvariantCulture);
            var result2 = discount2.Value.ToString("F2", CultureInfo.InvariantCulture);
            var result3 = discount3.Value.ToString("F2", CultureInfo.InvariantCulture);

            // Assert
            result1.Should().Be("15.50");
            result2.Should().Be("15.00");
            result3.Should().Be("15.56");
        }
    }
}