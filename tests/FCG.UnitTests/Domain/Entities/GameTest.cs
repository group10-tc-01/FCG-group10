using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FluentAssertions;
using FCG.Domain.ValueObjects;
using System;

namespace FCG.UnitTests.Domain.Entities
{
    public class GameTests
    {
        [Fact]
        public void Given_ValidGameParameters_When_CreateMethodIsCalled_Then_GameIsInstantiatedCorrectly()
        {
            // Arrange
            var gameBuilder = GameBuilder.Build();

            // Act
            var game = Game.Create(
                Name.Create(gameBuilder.Name),
                gameBuilder.Description,
                Price.Create(gameBuilder.Price),
                gameBuilder.Category
            );

            // Assert
            game.Should().NotBeNull();
            game.Id.Should().NotBe(Guid.Empty);
            game.Name.Value.Should().Be(gameBuilder.Name);
            game.Description.Should().Be(gameBuilder.Description);
            game.Price.Value.Should().Be(gameBuilder.Price);
            game.Category.Should().Be(gameBuilder.Category);
        }

        [Fact]
        public void Given_InvalidName_When_CreateGame_Then_ThrowsException()
        {
            // Arrange
            var gameBuilder = GameBuilder.Build();

            // Act & Assert
            // Nome inválido (menos de 2 caracteres)
            Action actShortName = () => Game.Create(
                Name.Create("A"),
                gameBuilder.Description,
                Price.Create(gameBuilder.Price),
                gameBuilder.Category
            );
            actShortName.Should().Throw<ArgumentException>()
                .WithMessage("Name must be at least 2 characters long.");

            // Nome nulo ou vazio
            Action actNullName = () => Game.Create(
                Name.Create(null),
                gameBuilder.Description,
                Price.Create(gameBuilder.Price),
                gameBuilder.Category
            );
            actNullName.Should().Throw<ArgumentException>()
                .WithMessage("Name cannot be null or empty.");
        }

        [Fact]
        public void Given_NullOrEmptyDescription_When_CreateGame_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var name = Name.Create("Test Game");
            var price = Price.Create(59.99m);
            var category = "Action";

            // Act & Assert
            Action actNullDescription = () => Game.Create(name, null, price, category);
            actNullDescription.Should().Throw<ArgumentException>()
                .WithMessage("Description cannot be null or empty.");

            Action actEmptyDescription = () => Game.Create(name, string.Empty, price, category);
            actEmptyDescription.Should().Throw<ArgumentException>()
                .WithMessage("Description cannot be null or empty.");
        }

        [Fact]
        public void Given_NullOrEmptyCategory_When_CreateGame_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var name = Name.Create("Test Game");
            var description = "Test Description";
            var price = Price.Create(59.99m);

            // Act & Assert
            Action actNullCategory = () => Game.Create(name, description, price, null);
            actNullCategory.Should().Throw<ArgumentException>()
                .WithMessage("Category cannot be null or empty.");

            Action actEmptyCategory = () => Game.Create(name, description, price, string.Empty);
            actEmptyCategory.Should().Throw<ArgumentException>()
                .WithMessage("Category cannot be null or empty.");
        }

        [Fact]
        public void Given_NegativePrice_When_CreateGame_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var name = Name.Create("Test Game");
            var description = "Test Description";
            var negativePrice = Price.Create(-10m); // Preço negativo
            var category = "Action";

            // Act
            Action act = () => Game.Create(name, description, negativePrice, category);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Price cannot be negative.");
        }

        [Fact]
        public void Given_TwoGamesWithSameProperties_When_Compare_Then_ShouldHaveSameProperties()
        {
            // Arrange
            var game1 = Game.Create(Name.Create("FIFA"), "Soccer game", Price.Create(59.99m), "Sports");
            var game2 = Game.Create(Name.Create("FIFA"), "Soccer game", Price.Create(59.99m), "Sports");

            // Assert
            game1.Should().Be(game2); // Verifica se a igualdade de referência é a mesma. Se a classe Game não sobrescrever Equals, isso vai falhar.
            game1.Name.Value.Should().Be(game2.Name.Value);
            game1.Description.Should().Be(game2.Description);
            game1.Price.Value.Should().Be(game2.Price.Value);
            game1.Category.Should().Be(game2.Category);
        }
    }
}