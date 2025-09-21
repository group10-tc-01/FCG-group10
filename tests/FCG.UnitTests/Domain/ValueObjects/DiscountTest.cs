using FCG.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Globalization;
using Xunit;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class DiscountTests
    {
        [Fact]
        public void Given_ValidPercentages_When_CreateDiscount_Then_ShouldCreateSuccessfullyAndSetProperties()
        {

            decimal validPercentage = 25.5m;
            decimal zeroPercentage = 0m;
            decimal maxPercentage = 100m;


            var discount = Discount.Create(validPercentage);
            var zeroDiscount = Discount.Create(zeroPercentage);
            var maxDiscount = Discount.Create(maxPercentage);

            discount.Should().NotBeNull();
            discount.Value.Should().Be(validPercentage);
            zeroDiscount.Value.Should().Be(0);
            maxDiscount.Value.Should().Be(100);
        }

        [Fact]
        public void Given_InvalidPercentages_When_CreateDiscount_Then_ShouldThrowArgumentException()
        {

            decimal negativePercentage = -5m;
            decimal invalidPercentage = 101m;


            Action actNegative = () => Discount.Create(negativePercentage);
            Action actAbove100 = () => Discount.Create(invalidPercentage);


            actNegative.Should().Throw<ArgumentException>()
                .WithMessage("Discount must be between 0 and 100.");
            actAbove100.Should().Throw<ArgumentException>()
                .WithMessage("Discount must be between 0 and 100.");
        }

        [Fact]
        public void Given_DiscountObject_When_ImplicitlyConvertedToDecimal_Then_ShouldReturnValue()
        {

            var discount = Discount.Create(15.75m);

            decimal value = discount;

            value.Should().Be(15.75m);
        }

        [Fact]
        public void Given_DecimalValue_When_ImplicitlyConvertedToDiscount_Then_ShouldCreateDiscount()
        {

            decimal value = 30m;

            Discount discount = value;

            discount.Value.Should().Be(30m);
        }

        [Fact]
        public void Given_TwoDiscountsWithSameValue_When_Compare_Then_ShouldBeEqual()
        {

            var discount1 = Discount.Create(25m);
            var discount2 = Discount.Create(25m);

            discount1.Should().Be(discount2);
        }

        [Fact]
        public void Given_TwoDiscountsWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {

            var discount1 = Discount.Create(25m);
            var discount2 = Discount.Create(50m);

            discount1.Should().NotBe(discount2);
        }

        [Fact]
        public void Given_DiscountObject_When_CallToString_Then_ShouldReturnFormattedValue()
        {
            var discount1 = Discount.Create(15.5m);
            var discount2 = Discount.Create(15m);
            var discount3 = Discount.Create(15.555m);

            var result1 = discount1.Value.ToString("F2", CultureInfo.InvariantCulture);
            var result2 = discount2.Value.ToString("F2", CultureInfo.InvariantCulture);
            var result3 = discount3.Value.ToString("F2", CultureInfo.InvariantCulture);

            result1.Should().Be("15.50");
            result2.Should().Be("15.00");
            result3.Should().Be("15.56");
        }

        [Fact]
        public void Given_TwoDiscountsWithSameValue_When_Compare_Then_ShouldBeEqualButNotSameReference()
        {

            var discount1 = Discount.Create(25m);
            var discount2 = Discount.Create(25m);

            discount1.Should().Be(discount2);
            discount1.Should().NotBeSameAs(discount2);
        }
    }
}