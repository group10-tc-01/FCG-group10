using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class GameTests
    {
        [Fact]
        public void Given_ValidGameParameters_When_CreateMethodIsCalled_Then_GameIsInstantiatedCorrectly()
        {
            // Arrange
            var gameEntity = GameBuilder.Build();

            // Act
            var game = Game.Create(Name.Create(gameEntity.Name), gameEntity.Description, Price.Create(gameEntity.Price), gameEntity.Category);

            // Assert
            game.Should().NotBeNull();
            game.Id.Should().NotBe(Guid.Empty);
            game.Name.Value.Should().Be(gameEntity.Name);
            game.Description.Should().Be(gameEntity.Description);
            game.Price.Value.Should().Be(gameEntity.Price);
            game.Category.Should().Be(gameEntity.Category);
        }

        [Fact]
        public void Given_InvalidName_When_CreateGame_Then_ThrowsException()
        {
            // Arrange
            var gameEntity = GameBuilder.Build();

            // Act & Assert
            Action actShortName = () => Game.Create(Name.Create("A"), gameEntity.Description, Price.Create(gameEntity.Price),
                gameEntity.Category
            );

            actShortName.Should().Throw<DomainException>()
                .WithMessage("Name must be at least 2 characters long.");

            Action actNullName = () => Game.Create(
                Name.Create(""),
                gameEntity.Description,
                Price.Create(gameEntity.Price),
                gameEntity.Category
            );

            actNullName.Should().Throw<DomainException>().WithMessage("Name cannot be null or empty.");
        }

        [Fact]
        public void Given_NullOrEmptyDescription_When_CreateGame_Then_ShouldThrowDomainException()
        {
            // Arrange
            var name = Name.Create("Test Game");
            var price = Price.Create(59.99m);
            var category = "Action";

            // Act & Assert
            Action actNullDescription = () => Game.Create(name, "", price, category);
            actNullDescription.Should().Throw<DomainException>().WithMessage("Description cannot be null or empty.");

            Action actEmptyDescription = () => Game.Create(name, string.Empty, price, category);
            actEmptyDescription.Should().Throw<DomainException>().WithMessage("Description cannot be null or empty.");
        }

        [Fact]
        public void Given_NullOrEmptyCategory_When_CreateGame_Then_ShouldThrowDomainException()
        {
            // Arrange
            var name = Name.Create("Test Game");
            var description = "Test Description";
            var price = Price.Create(59.99m);

            // Act & Assert
            Action actNullCategory = () => Game.Create(name, description, price, "");
            actNullCategory.Should().Throw<DomainException>().WithMessage("Category cannot be null or empty.");

            Action actEmptyCategory = () => Game.Create(name, description, price, string.Empty);
            actEmptyCategory.Should().Throw<DomainException>().WithMessage("Category cannot be null or empty.");
        }
    }
}