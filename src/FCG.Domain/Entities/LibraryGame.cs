using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class LibraryGame : BaseEntity
    {
        public Guid LibraryId { get; private set; }
        public Guid GameId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public Price PurchasePrice { get; private set; } = null!;
        public GameStatus Status { get; private set; }
        public Library? Library { get; }
        public Game? Game { get; }

        private LibraryGame(Guid libraryId, Guid gameId, Price purchasePrice)
        {
            LibraryId = libraryId;
            GameId = gameId;
            PurchaseDate = DateTime.UtcNow;
            PurchasePrice = purchasePrice;
            Status = GameStatus.Active;
        }

        private LibraryGame() { }

        public static LibraryGame Create(Guid libraryId, Guid gameId, Price purchasePrice)
        {
            if (libraryId == Guid.Empty)
            {
                throw new DomainException("LibraryId cannot be empty.");
            }
            if (gameId == Guid.Empty)
            {
                throw new DomainException("GameId cannot be empty.");
            }
            return new LibraryGame(libraryId, gameId, purchasePrice);
        }

        public void Suspend()
        {
            if (Status == GameStatus.Suspended)
            {
                return;
            }
            Status = GameStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (Status == GameStatus.Active)
            {
                return;
            }
            Status = GameStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
