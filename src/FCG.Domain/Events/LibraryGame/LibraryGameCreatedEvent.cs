namespace FCG.Domain.Events.LibraryGame
{
    public class LibraryGameCreatedEvent : LibraryGameBaseEvent
    {
        public Guid LibraryId { get; set; }
        public Guid GameId { get; set; }
        public decimal PurchasePrice { get; set; }

        public LibraryGameCreatedEvent(Guid libraryGameId, Guid libraryId, Guid gameId, decimal purchasePrice)
            : base(libraryGameId)
        {
            LibraryId = libraryId;
            GameId = gameId;
            PurchasePrice = purchasePrice;
        }
    }
}
