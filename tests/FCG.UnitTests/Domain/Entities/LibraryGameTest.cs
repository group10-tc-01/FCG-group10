using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class LibraryGameTests
    {
        [Fact]
        public void Given_ValidParameters_When_CreateLibraryGame_Then_ShouldCreateSuccessfullyAndSetProperties()
        {
            // Arrange
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var purchasePrice = Price.Create(29.99m);
            var beforeCreate = DateTime.UtcNow;

            // Act
            var libraryGame = LibraryGame.Create(libraryId, gameId, purchasePrice);
            var afterCreate = DateTime.UtcNow;

            // Assert
            libraryGame.Should().NotBeNull();
            libraryGame.Id.Should().NotBe(Guid.Empty);
            libraryGame.LibraryId.Should().Be(libraryId);
            libraryGame.GameId.Should().Be(gameId);
            libraryGame.PurchasePrice.Value.Should().Be(purchasePrice);
            libraryGame.PurchaseDate.Should().BeOnOrAfter(beforeCreate);
            libraryGame.PurchaseDate.Should().BeOnOrBefore(afterCreate);
            libraryGame.Status.Should().Be(GameStatus.Active);
        }

        [Fact]
        public void Given_EmptyLibraryId_When_CreateLibraryGame_Then_ShouldThrowDomainException()
        {
            // Arrange
            var emptyLibraryId = Guid.Empty;
            var validGameId = Guid.NewGuid();
            var validPrice = Price.Create(19.99m);

            // Act
            Action act = () => LibraryGame.Create(emptyLibraryId, validGameId, validPrice);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("LibraryId cannot be empty.");
        }

        [Fact]
        public void Given_EmptyGameId_When_CreateLibraryGame_Then_ShouldThrowDomainException()
        {
            // Arrange
            var validLibraryId = Guid.NewGuid();
            var emptyGameId = Guid.Empty;
            var validPrice = Price.Create(19.99m);

            // Act
            Action act = () => LibraryGame.Create(validLibraryId, emptyGameId, validPrice);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("GameId cannot be empty.");
        }
        [Fact]
        public void Given_NegativePrice_When_CreateLibraryGame_Then_ShouldThrowDomainException()
        {
            // Arrange
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var negativePrice = -10.0m;

            // Act & Assert
            Action act = () => LibraryGame.Create(libraryId, gameId, negativePrice);
            act.Should().Throw<DomainException>().WithMessage(ResourceMessages.PriceCannotBeNegative);
        }

        [Fact]
        public void Given_HighPrecisionPrice_When_CreateLibraryGame_Then_ShouldMaintainPrecision()
        {
            // Arrange
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var precisePrice = 29.999999m;

            // Act
            var libraryGame = LibraryGame.Create(libraryId, gameId, precisePrice);

            // Assert
            libraryGame.PurchasePrice.Value.Should().Be(29.999999m);
        }
        [Fact]
        public void Suspend_WhenStatusIsActive_ShouldChangeStatusToSuspended()
        {
            // Arrange
            var libraryGame = LibraryGame.Create(Guid.NewGuid(), Guid.NewGuid(), Price.Create(10m));
            libraryGame.Status.Should().Be(GameStatus.Active);

            // Act
            libraryGame.Suspend();

            // Assert
            libraryGame.Status.Should().Be(GameStatus.Suspended);
        }

        [Fact]
        public void Activate_WhenStatusIsSuspended_ShouldChangeStatusToActive()
        {
            // Arrange
            var libraryGame = LibraryGame.Create(Guid.NewGuid(), Guid.NewGuid(), Price.Create(10m));
            libraryGame.Suspend();
            libraryGame.Status.Should().Be(GameStatus.Suspended);

            // Act
            libraryGame.Activate();

            // Assert
            libraryGame.Status.Should().Be(GameStatus.Active);
        }
    }
}