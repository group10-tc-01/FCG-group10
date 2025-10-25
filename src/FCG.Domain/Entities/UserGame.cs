using FCG.Domain.Enum;
using FCG.Domain.Exceptions;

namespace FCG.Domain.Entities
{
    public sealed class UserGame : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid GameId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public GameStatus Status { get; private set; }
        public User User { get; private set; }
        public Game Game { get; private set; }

        private UserGame()
        {
        }

        private UserGame(Guid userId, Guid gameId, DateTime purchaseDate, GameStatus status)
        {
            UserId = userId;
            GameId = gameId;
            PurchaseDate = purchaseDate;
            Status = GameStatus.Active;
        }
        public static UserGame Create(Guid userId, Guid gameId, DateTime purchaseDate)
        {
            if (userId == Guid.Empty)
            {
                throw new DomainException("UserId cannot be empty.");
            }
            if (gameId == Guid.Empty)
            {
                throw new DomainException("GameId cannot be empty.");
            }

            if (purchaseDate > DateTime.UtcNow)
            {
                throw new DomainException("Purchase date cannot be in the future.");
            }

            return new UserGame(userId, gameId, purchaseDate, GameStatus.Active);
        }
    }
}