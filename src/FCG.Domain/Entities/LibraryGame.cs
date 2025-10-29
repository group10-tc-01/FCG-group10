using FCG.Domain.Enum;
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
            return new LibraryGame(libraryId, gameId, purchasePrice);
        }

        public void Suspended()
        {
            Status = GameStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            Status = GameStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
