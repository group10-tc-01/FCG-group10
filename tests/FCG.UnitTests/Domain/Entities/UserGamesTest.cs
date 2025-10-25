using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Events.UserGame;
using FCG.Domain.Exceptions;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class UserGamesTest
    {
        [Fact]
        public void Create_GivenFuturePurchaseDate_Should_ThrowDomainException()
        {
            // Given (Arrange)
            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var futureDate = DateTime.UtcNow.AddDays(1);

            // When (Act)
            Action act = () => UserGame.Create(userId, gameId, futureDate);

            // Then (Assert)
            act.Should().Throw<DomainException>()
                .WithMessage("Purchase date cannot be in the future.");
        }
        [Fact]
        public void Create_GivenValidParameters_Should_ReturnNewUserGameWithActiveStatus_And_RaiseDomainEvent()
        {
            // Given (Arrange)
            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var purchaseDate = DateTime.UtcNow.AddDays(-1);

            // When (Act)
            var userGame = UserGame.Create(userId, gameId, purchaseDate);

            // Then (Assert)
            userGame.Should().NotBeNull();
            userGame.UserId.Should().Be(userId);
            userGame.GameId.Should().Be(gameId);
            userGame.PurchaseDate.Should().Be(purchaseDate);
            userGame.Status.Should().Be(GameStatus.Active);
            userGame.Id.Should().NotBe(Guid.Empty);

            userGame.DomainEvents.Should().NotBeNullOrEmpty();
            userGame.DomainEvents.Should().HaveCount(1);

            var domainEvent = userGame.DomainEvents.First();
            domainEvent.Should().BeOfType<UserGameCreatedEvent>();

            var gameEvent = (UserGameCreatedEvent)domainEvent;
            gameEvent.UserId.Should().Be(userId);
            gameEvent.GameId.Should().Be(gameId);
        }
        [Fact]
        public void Create_GivenValidParameters_Should_ReturnNewUserGameWithActiveStatus()
        {
            // Given (Arrange)
            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var purchaseDate = DateTime.UtcNow.AddDays(-1);

            // When (Act)
            var userGame = UserGame.Create(userId, gameId, purchaseDate);

            // Then (Assert)
            userGame.Should().NotBeNull();
            userGame.UserId.Should().Be(userId);
            userGame.GameId.Should().Be(gameId);
            userGame.PurchaseDate.Should().Be(purchaseDate);
            userGame.Status.Should().Be(GameStatus.Active);
            userGame.Id.Should().NotBe(Guid.Empty);
        }
        [Fact]
        public void Create_GivenEmptyUserId_Should_ThrowDomainException()
        {
            // Given (Arrange)
            var emptyUserId = Guid.Empty;
            var validGameId = Guid.NewGuid();
            var purchaseDate = DateTime.UtcNow;

            // When (Act)
            Action act = () => UserGame.Create(emptyUserId, validGameId, purchaseDate);

            // Then (Assert)
            act.Should().Throw<DomainException>()
               .WithMessage("UserId cannot be empty.");
        }

        [Fact]
        public void Create_GivenEmptyGameId_Should_ThrowDomainException()
        {
            // Given (Arrange)
            var validUserId = Guid.NewGuid();
            var emptyGameId = Guid.Empty;
            var purchaseDate = DateTime.UtcNow;

            // When (Act)
            Action act = () => UserGame.Create(validUserId, emptyGameId, purchaseDate);

            // Then (Assert)
            act.Should().Throw<DomainException>()
               .WithMessage("GameId cannot be empty.");
        }
    }
}