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
        public void Given_ZeroPrice_When_CreatePrice_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            decimal zeroPrice = 0m;

            // Act
            var price = Price.Create(zeroPrice);

            // Assert
            price.Value.Should().Be(0);
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
            decimal largePrice = decimal.MaxValue;

            var price = Price.Create(largePrice);

            price.Value.Should().Be(decimal.MaxValue);
        }

        [Fact]
        public void Given_NegativePrice_When_CreatePrice_Then_ShouldThrowArgumentException()
        {

            decimal negativePrice = -10.50m;


            Action act = () => Price.Create(negativePrice);

            act.Should().Throw<ArgumentException>()
               .WithMessage("The price cannot be a negative value. (Parameter 'value')");
        }

        [Fact]
        public void Given_PriceObject_When_ImplicitConvertToDecimal_Then_ShouldReturnValue()
        {

            var price = Price.Create(29.99m);

            decimal value = price;

            value.Should().Be(29.99m);
        }

        [Fact]
        public void Given_DecimalValue_When_ImplicitConvertToPrice_Then_ShouldCreatePrice()
        {

            decimal value = 49.99m;

            Price price = value;

            price.Value.Should().Be(49.99m);
        }

        [Fact]
        public void Given_TwoPricesWithSameValue_When_Compare_Then_ShouldBeEqual()
        {
            var price1 = Price.Create(19.99m);
            var price2 = Price.Create(19.99m);

            price1.Should().Be(price2);
            price1.GetHashCode().Should().Be(price2.GetHashCode());
        }

        [Fact]
        public void Given_TwoPricesWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {
            var price1 = Price.Create(19.99m);
            var price2 = Price.Create(29.99m);

            price1.Should().NotBe(price2);
        }

        [Fact]
        public void Given_PriceWithHighPrecision_When_CreatePrice_Then_ShouldMaintainPrecision()
        {

            decimal precisePrice = 19.999999m;


            var price = Price.Create(precisePrice);


            price.Value.Should().Be(19.999999m);
        }
    }
}
