using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class LibraryGame : BaseEntity
    {
        public Guid LibraryId { get; private set; }
        public Guid GameId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public Price PurchasePrice { get; private set; } = null!;
        public Library? Library { get; }
        public Game? Game { get; }

        private LibraryGame(Guid libraryId, Guid gameId, Price purchasePrice)
        {
            LibraryId = libraryId;
            GameId = gameId;
            PurchaseDate = DateTime.UtcNow;
            PurchasePrice = purchasePrice;
        }

        private LibraryGame() { }

        public static LibraryGame Create(Guid libraryId, Guid gameId, Price purchasePrice)
        {
            return new LibraryGame(libraryId, gameId, purchasePrice);
        }
    }
}
