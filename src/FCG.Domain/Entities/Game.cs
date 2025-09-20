using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class Game : BaseEntity
    {
        public Name Name { get; private set; }
        public string Description { get; private set; }
        public Price Price { get; private set; }
        public string Category { get; private set; }

        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<LibraryGame> LibraryGames { get; set; }

        public Game()
        {
            Promotions = new List<Promotion>();
            LibraryGames = new List<LibraryGame>();
        }

        private Game(Name name, string description, Price price, string category)
        {
            if (price.Value < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category cannot be null or empty.");
            }

            Name = name;
            Description = description;
            Price = price;
            Category = category;

            Promotions = new List<Promotion>();
            LibraryGames = new List<LibraryGame>();
        }

        public static Game Create(Name name, string description, Price price, string category)
        {
            return new Game(name, description, price, category);
        }
    }
}