using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;

namespace FCG.Domain.Entities
{
    public sealed class Game : BaseEntity
    {
        public Name Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public Price Price { get; private set; } = null!;
        public GameCategory Category { get; private set; }

        public ICollection<Promotion>? Promotions { get; }
        public ICollection<LibraryGame>? LibraryGames { get; }

        private Game(Name name, string description, Price price, GameCategory category)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new DomainException(ResourceMessages.DescriptionCannotBeNullOrEmpty);
            }

            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        private Game() { }

        public static Game Create(Name name, string description, Price price, GameCategory category)
        {
            return new Game(name, description, price, category);
        }
    }
}