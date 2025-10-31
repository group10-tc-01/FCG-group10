using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class GameTests
    {
        [Fact]
        public void Given_ValidGameParameters_When_Create_Then_ShouldInstantiateGameCorrectly()
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
        public void Given_InvalidName_When_Create_Then_ShouldThrowDomainException()
        {
            // Arrange
            var gameEntity = GameBuilder.Build();
            var actShortName = () => Game.Create(Name.Create("A"), gameEntity.Description, Price.Create(gameEntity.Price), gameEntity.Category);
            var actNullName = () => Game.Create(Name.Create(""), gameEntity.Description, Price.Create(gameEntity.Price), gameEntity.Category);

            // Act & Assert
            actShortName.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.NameMinimumLength);

            actNullName.Should().Throw<DomainException>().WithMessage(ResourceMessages.NameCannotBeNullOrEmpty);
        }

        [Fact]
        public void Given_NullOrEmptyDescription_When_Create_Then_ShouldThrowDomainException()
        {
            // Arrange
            var name = Name.Create("Test Game");
            var price = Price.Create(59.99m);
            var category = GameCategory.Action;
            var actNullDescription = () => Game.Create(name, "", price, category);
            var actEmptyDescription = () => Game.Create(name, string.Empty, price, category);

            // Act & Assert
            actNullDescription.Should().Throw<DomainException>().WithMessage(ResourceMessages.DescriptionCannotBeNullOrEmpty);
            actEmptyDescription.Should().Throw<DomainException>().WithMessage(ResourceMessages.DescriptionCannotBeNullOrEmpty);
        }
    }
}