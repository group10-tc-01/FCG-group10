using FCG.Domain.Enum;

namespace FCG.Application.UseCases.Users.MyGames
{
    public class LibraryGameResponse
    {
        public Guid GameId { get; set; }
        public string GameName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public GameStatus Status { get; set; }
    }
}
