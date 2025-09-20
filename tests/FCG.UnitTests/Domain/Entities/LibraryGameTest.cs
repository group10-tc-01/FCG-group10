using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
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
            var purchasePrice = 29.99m;
            var beforeCreate = DateTime.UtcNow;

            // Act
            var libraryGame = LibraryGame.Create(libraryId, gameId, purchasePrice);
            var afterCreate = DateTime.UtcNow;

            // Assert
            libraryGame.Should().NotBeNull();
            libraryGame.Id.Should().NotBe(Guid.Empty);
            libraryGame.LibraryId.Should().Be(libraryId);
            libraryGame.GameId.Should().Be(gameId);
            libraryGame.PurchasePrice.Should().Be(purchasePrice);
            libraryGame.PurchaseDate.Should().BeOnOrAfter(beforeCreate);
            libraryGame.PurchaseDate.Should().BeOnOrBefore(afterCreate);
        }

        [Fact]
        public void Given_EmptyLibraryId_When_CreateLibraryGame_Then_ShouldCreateWithEmptyLibraryId()
        {
            // Arrange
            var emptyLibraryId = Guid.Empty;
            var gameId = Guid.NewGuid();
            var purchasePrice = 19.99m;

            // Act
            var libraryGame = LibraryGame.Create(emptyLibraryId, gameId, purchasePrice);

            // Assert
            libraryGame.LibraryId.Should().Be(Guid.Empty);
            libraryGame.GameId.Should().Be(gameId);
            libraryGame.PurchasePrice.Should().Be(purchasePrice);
        }


        [Fact]
        public void Given_ZeroPrice_When_CreateLibraryGame_Then_ShouldCreateWithZeroPrice()
        {
            // Arrange
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var zeroPrice = 0m;

            // Act
            var libraryGame = LibraryGame.Create(libraryId, gameId, zeroPrice);

            // Assert
            libraryGame.PurchasePrice.Should().Be(0m);
        }

        [Fact]
        public void Given_NegativePrice_When_CreateLibraryGame_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var negativePrice = -10.0m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => LibraryGame.Create(libraryId, gameId, negativePrice));
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
            libraryGame.PurchasePrice.Should().Be(29.999999m);
        }

        [Fact]
        public void Given_LibraryGame_When_SetLibrary_Then_ShouldSetLibraryProperty()
        {
            // Arrange
            var libraryGame = LibraryGame.Create(Guid.NewGuid(), Guid.NewGuid(), 25.99m);
            var library = Library.Create(Guid.NewGuid());

            // Act
            libraryGame.Library = library;

            // Assert
            libraryGame.Library.Should().Be(library);
        }
    }
}