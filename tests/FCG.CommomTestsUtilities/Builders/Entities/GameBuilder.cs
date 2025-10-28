using Bogus;
using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public static class GameBuilder
    {
        public static Game Build()
        {
            return new Faker<Game>().CustomInstantiator(f =>
                Game.Create(Name.Create(f.Commerce.ProductName()),
                    f.Commerce.ProductDescription(),
                    Price.Create(f.Random.Decimal(1, 100)),
                    f.Commerce.Categories(1)[0]
                )
            ).Generate();
        }
    }
}