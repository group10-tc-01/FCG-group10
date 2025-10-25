using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class Game : BaseEntity
    {
        public Name Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public Price Price { get; private set; } = null!;
        public string Category { get; private set; } = null!;

        public ICollection<Promotion>? Promotions { get; }
        public ICollection<LibraryGame>? LibraryGames { get; }
        public ICollection<UserGame>? UserGames { get; }
        private Game(Name name, string description, Price price, string category)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new DomainException("Description cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                throw new DomainException("Category cannot be null or empty.");
            }

            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        private Game() { }

        public static Game Create(Name name, string description, Price price, string category)
        {
            return new Game(name, description, price, category);
        }
    }
}