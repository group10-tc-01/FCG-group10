namespace FCG.Domain.Entities
{
    public class Game : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Price Price { get; private set; }
        public string Category { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public bool IsActive { get; private set; } = true;

        public ICollection<Promotion> Promotions { get; set; }

        public ICollection<LibraryGame> LibraryGames { get; set; }

        protected Game() { }
        public Game(string name, string description, Price price, string category)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
