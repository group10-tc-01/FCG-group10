namespace FCG.Domain.Entities
{
    public sealed class LibraryGame : BaseEntity
    {
        public Guid LibraryId { get; private set; }
        public Guid GameId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public decimal PurchasePrice { get; private set; }


        public Library Library { get; set; }
        public Game Game { get; set; }

        public LibraryGame()
        {
        }
        private LibraryGame(Guid libraryId, Guid gameId, decimal purchasePrice)
        {
            if (purchasePrice < 0)
            {
                throw new ArgumentException("Purchase price cannot be negative.");
            }
            LibraryId = libraryId;
            GameId = gameId;
            PurchaseDate = DateTime.UtcNow;
            PurchasePrice = purchasePrice;
        }
        public static LibraryGame Create(Guid libraryId, Guid gameId, decimal purchasePrice)
        {
            return new LibraryGame(libraryId, gameId, purchasePrice);
        }


    }
}
