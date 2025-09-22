using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class PromotionTests
    {
        [Fact]
        public void Given_ValidParameters_When_CreatePromotion_Then_AllPropertiesShouldBeSetCorrectly()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var discountValue = 25.5m;
            var startDate = DateTime.UtcNow.Date;
            var endDate = DateTime.UtcNow.Date.AddDays(7);
            var preciseDiscount = 12.75m;

            // Act
            var promotion = Promotion.Create(gameId, discountValue, startDate, endDate);
            var promotionPrecise = Promotion.Create(gameId, preciseDiscount, startDate, endDate);

            // Assert
            promotion.Should().NotBeNull();
            promotion.Id.Should().NotBe(Guid.Empty);
            promotion.GameId.Should().Be(gameId);
            promotion.Discount.Value.Should().Be(discountValue);
            promotion.StartDate.Should().Be(startDate);
            promotion.EndDate.Should().Be(endDate);

            // Verifica o valor de precisão
            promotionPrecise.Discount.Value.Should().Be(preciseDiscount);
        }

        [Fact]
        public void Given_DiscountValues_When_CreatePromotion_Then_ShouldHandleZeroAndMaxDiscount()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.Date;
            var endDate = DateTime.UtcNow.Date.AddDays(3);
            var zeroDiscount = 0m;
            var maxDiscount = 100m;

            // Act
            var zeroPromotion = Promotion.Create(gameId, zeroDiscount, startDate, endDate);
            var maxPromotion = Promotion.Create(gameId, maxDiscount, startDate, endDate);

            // Assert
            zeroPromotion.Discount.Value.Should().Be(0m);
            maxPromotion.Discount.Value.Should().Be(100m);
        }

        [Fact]
        public void Given_EndDateBeforeStartDate_When_CreatePromotion_Then_ShouldThrowDomainException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var discount = 20m;
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(-1);

            // Act
            Action act = () => Promotion.Create(gameId, discount, startDate, endDate);

            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("End date must be on or after the start date.");
        }

        [Fact]
        public void Given_DiscountOutOfRange_When_CreatePromotion_Then_ShouldThrowDomainException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(2);
            var negativeDiscount = -5m;
            var invalidDiscount = 101m;

            // Act
            Action actNegative = () => Promotion.Create(gameId, negativeDiscount, startDate, endDate);
            Action actAbove100 = () => Promotion.Create(gameId, invalidDiscount, startDate, endDate);

            // Assert
            actNegative.Should().Throw<DomainException>()
                       .WithMessage("Discount must be between 0 and 100.");
            actAbove100.Should().Throw<DomainException>()
                       .WithMessage("Discount must be between 0 and 100.");
        }

        [Fact]
        public void Given_SameStartAndEndDate_When_CreatePromotion_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var discount = 30m;
            var sameDate = DateTime.UtcNow.Date;

            // Act
            var promotion = Promotion.Create(gameId, discount, sameDate, sameDate);

            // Assert
            promotion.StartDate.Should().Be(sameDate);
            promotion.EndDate.Should().Be(sameDate);
        }

        [Fact]
        public void Given_TwoPromotionsWithSameData_When_Compare_Then_ShouldHaveDifferentIds()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var discount = 25m;
            var startDate = DateTime.UtcNow.Date;
            var endDate = DateTime.UtcNow.Date.AddDays(7);

            // Act
            var promotion1 = Promotion.Create(gameId, discount, startDate, endDate);
            var promotion2 = Promotion.Create(gameId, discount, startDate, endDate);

            // Assert
            promotion1.Id.Should().NotBe(promotion2.Id);
            promotion1.GameId.Should().Be(promotion2.GameId);
            promotion1.Discount.Should().Be(promotion2.Discount);
            promotion1.StartDate.Should().Be(promotion2.StartDate);
            promotion1.EndDate.Should().Be(promotion2.EndDate);
        }

    }
}