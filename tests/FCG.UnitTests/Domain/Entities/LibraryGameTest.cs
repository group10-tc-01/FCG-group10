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
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var purchasePrice = 29.99m;
            var beforeCreate = DateTime.UtcNow;

            var libraryGame = LibraryGame.Create(libraryId, gameId, purchasePrice);
            var afterCreate = DateTime.UtcNow;

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
            var emptyLibraryId = Guid.Empty;
            var gameId = Guid.NewGuid();
            var purchasePrice = 19.99m;

            var libraryGame = LibraryGame.Create(emptyLibraryId, gameId, purchasePrice);

            libraryGame.LibraryId.Should().Be(Guid.Empty);
            libraryGame.GameId.Should().Be(gameId);
            libraryGame.PurchasePrice.Should().Be(purchasePrice);
        }


        [Fact]
        public void Given_ZeroPrice_When_CreateLibraryGame_Then_ShouldCreateWithZeroPrice()
        {
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var zeroPrice = 0m;

            var libraryGame = LibraryGame.Create(libraryId, gameId, zeroPrice);

            libraryGame.PurchasePrice.Should().Be(0m);
        }

        [Fact]
        public void Given_NegativePrice_When_CreateLibraryGame_Then_ShouldThrowArgumentException()
        {
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var negativePrice = -10.0m;

            Assert.Throws<ArgumentException>(() => LibraryGame.Create(libraryId, gameId, negativePrice));
        }

        [Fact]
        public void Given_HighPrecisionPrice_When_CreateLibraryGame_Then_ShouldMaintainPrecision()
        {
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var precisePrice = 29.999999m;

            var libraryGame = LibraryGame.Create(libraryId, gameId, precisePrice);

            libraryGame.PurchasePrice.Should().Be(29.999999m);
        }

        [Fact]
        public void Given_LibraryGame_When_SetLibrary_Then_ShouldSetLibraryProperty()
        {
            var libraryGame = LibraryGame.Create(Guid.NewGuid(), Guid.NewGuid(), 25.99m);
            var library = Library.Create(Guid.NewGuid());

            libraryGame.Library = library;

            libraryGame.Library.Should().Be(library);
        }
    }
}