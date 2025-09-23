namespace FCG.Domain.Events.Game
{
    public class GameCreatedEvent : GameBaseEvent
    {
        public string GameName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public GameCreatedEvent(Guid gameId, string gameName, decimal price, string category) : base(gameId)
        {
            GameName = gameName;
            Price = price;
            Category = category;
        }
    }
}
