namespace FCG.Domain.Entities
{
    public class LibraryGame : BaseEntity
    {
        public Guid LibraryId { get; private set; }
        public Guid GameId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public decimal PurchasePrice { get; private set; }


        public Library Library { get; set; }
        public Game Game { get; set; }

        protected LibraryGame()
        {
        }
        public LibraryGame(Guid libraryId, Guid gameId, decimal purchasePrice)
        {
            LibraryId = libraryId;
            GameId = gameId;
            PurchaseDate = DateTime.UtcNow;
            PurchasePrice = purchasePrice;
        }
    }
}
