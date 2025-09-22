using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class Game : BaseEntity
    {
        public Name Name { get; private set; }
        public string Description { get; private set; }
        public Price Price { get; private set; }
        public string Category { get; private set; }

        public ICollection<Promotion>? Promotions { get; private set; }
        public ICollection<LibraryGame>? LibraryGames { get; private set; }

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

        public static Game Create(Name name, string description, Price price, string category)
        {
            return new Game(name, description, price, category);
        }
    }
}