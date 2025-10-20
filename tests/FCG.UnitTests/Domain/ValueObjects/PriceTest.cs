using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FluentAssertions;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class PriceTests
    {
        [Fact]
        public void Given_ValidPrice_When_CreatePrice_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            decimal validPrice = 59.99m;

            // Act
            var price = Price.Create(validPrice);

            // Assert
            price.Should().NotBeNull();
            price.Value.Should().Be(validPrice);
        }

        [Fact]
        public void Given_VerySmallPrice_When_CreatePrice_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            decimal smallPrice = 0.01m;

            // Act
            var price = Price.Create(smallPrice);

            // Assert
            price.Value.Should().Be(0.01m);
        }

        [Fact]
        public void Given_LargePrice_When_CreatePrice_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            decimal largePrice = decimal.MaxValue;

            // Act
            var price = Price.Create(largePrice);

            // Assert
            price.Value.Should().Be(decimal.MaxValue);
        }

        [Fact]
        public void Given_NegativePrice_When_CreatePrice_Then_ShouldThrowDomainException()
        {
            // Arrange
            decimal negativePrice = -10.50m;

            // Act
            Action act = () => Price.Create(negativePrice);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("The price cannot be a negative value.");
        }

        [Fact]
        public void Given_PriceObject_When_ImplicitConvertToDecimal_Then_ShouldReturnValue()
        {
            // Arrange
            var price = Price.Create(29.99m);

            // Act
            decimal value = price;

            // Assert
            value.Should().Be(29.99m);
        }

        [Fact]
        public void Given_DecimalValue_When_ImplicitConvertToPrice_Then_ShouldCreatePrice()
        {
            // Arrange
            decimal value = 49.99m;

            // Act
            Price price = value;

            // Assert
            price.Value.Should().Be(49.99m);
        }

        [Fact]
        public void Given_PriceWithHighPrecision_When_CreatePrice_Then_ShouldMaintainPrecision()
        {
            // Arrange
            decimal precisePrice = 19.999999m;

            // Act
            var price = Price.Create(precisePrice);

            // Assert
            price.Value.Should().Be(19.999999m);
        }
    }
}
