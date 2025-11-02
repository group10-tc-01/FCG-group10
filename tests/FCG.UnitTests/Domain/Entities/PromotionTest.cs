using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class PromotionTests
    {
        [Fact]
        public void Given_ValidParameters_When_Create_Then_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var game = GameBuilder.Build();
            var discountValue = 25.5m;
            var startDate = DateTime.UtcNow.Date;
            var endDate = DateTime.UtcNow.Date.AddDays(7);

            // Act
            var promotion = Promotion.Create(game.Id, discountValue, startDate, endDate);

            // Assert
            promotion.Should().NotBeNull();
            promotion.Id.Should().NotBe(Guid.Empty);
            promotion.GameId.Should().Be(game.Id);
            promotion.Discount.Value.Should().Be(discountValue);
            promotion.StartDate.Should().Be(startDate);
            promotion.EndDate.Should().Be(endDate);
        }

        [Fact]
        public void Given_ZeroAndMaxDiscount_When_Create_Then_ShouldHandleBothValues()
        {
            // Arrange
            var game = GameBuilder.Build();
            var startDate = DateTime.UtcNow.Date;
            var endDate = DateTime.UtcNow.Date.AddDays(3);
            var zeroDiscount = 0m;
            var maxDiscount = 100m;

            // Act
            var zeroPromotion = Promotion.Create(game.Id, zeroDiscount, startDate, endDate);
            var maxPromotion = Promotion.Create(game.Id, maxDiscount, startDate, endDate);

            // Assert
            zeroPromotion.Discount.Value.Should().Be(0m);
            maxPromotion.Discount.Value.Should().Be(100m);
        }

        [Fact]
        public void Given_EndDateBeforeStartDate_When_Create_Then_ShouldThrowDomainException()
        {
            // Arrange
            var game = GameBuilder.Build();
            var discount = 20m;
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(-1);
            var act = () => Promotion.Create(game.Id, discount, startDate, endDate);

            // Act & Assert
            act.Should().Throw<DomainException>()
               .WithMessage(ResourceMessages.PromotionEndDateMustBeAfterStartDate);
        }

        [Fact]
        public void Given_DiscountOutOfRange_When_Create_Then_ShouldThrowDomainException()
        {
            // Arrange
            var game = GameBuilder.Build();
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(2);
            var negativeDiscount = -5m;
            var invalidDiscount = 101m;
            var actNegative = () => Promotion.Create(game.Id, negativeDiscount, startDate, endDate);
            var actAbove100 = () => Promotion.Create(game.Id, invalidDiscount, startDate, endDate);

            // Act & Assert
            actNegative.Should().Throw<DomainException>()
                       .WithMessage(ResourceMessages.DiscountMustBeBetweenZeroAndHundred);
            actAbove100.Should().Throw<DomainException>()
                       .WithMessage(ResourceMessages.DiscountMustBeBetweenZeroAndHundred);
        }

        [Fact]
        public void Given_SameStartAndEndDate_When_Create_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            var game = GameBuilder.Build();
            var discount = 30m;
            var sameDate = DateTime.UtcNow.Date;

            // Act
            var promotion = Promotion.Create(game.Id, discount, sameDate, sameDate);

            // Assert
            promotion.StartDate.Should().Be(sameDate);
            promotion.EndDate.Should().Be(sameDate);
        }

        [Fact]
        public void Given_TwoPromotionsWithSameData_When_Create_Then_ShouldHaveDifferentIds()
        {
            // Arrange
            var game = GameBuilder.Build();
            var discount = 25m;
            var startDate = DateTime.UtcNow.Date;
            var endDate = DateTime.UtcNow.Date.AddDays(7);

            // Act
            var promotion1 = Promotion.Create(game.Id, discount, startDate, endDate);
            var promotion2 = Promotion.Create(game.Id, discount, startDate, endDate);

            // Assert
            promotion1.Id.Should().NotBe(promotion2.Id);
            promotion1.GameId.Should().Be(promotion2.GameId);
            promotion1.Discount.Should().Be(promotion2.Discount);
            promotion1.StartDate.Should().Be(promotion2.StartDate);
            promotion1.EndDate.Should().Be(promotion2.EndDate);
        }

    }
}