using Bogus;
using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public static class GameBuilder
    {
        public static Game Build()
        {
            return new Faker<Game>().CustomInstantiator(f => Game.Create(f.Commerce.ProductName(), f.Commerce.ProductDescription(), f.Random.Decimal(1, 100), f.Commerce.Categories(1)[0])).Generate();
        }
        public static List<Game> BuildList(int count)
        {
            return new Faker<Game>()
                .CustomInstantiator(f => Game.Create(
                    Name.Create(f.Commerce.ProductName()),
                    f.Commerce.ProductDescription(),
                    Price.Create(f.Random.Decimal(1, 100)),
                    f.Commerce.Categories(1)[0]
                ))
                .Generate(count);
        }

        public static Game BuildWithName(string name)
        {
            var faker = new Faker();
            return Game.Create(
                Name.Create(name),
                faker.Commerce.ProductDescription(),
                Price.Create(faker.Random.Decimal(1, 100)),
                faker.Commerce.Categories(1)[0]
            );
        }

        public static Game BuildWithCategory(string category)
        {
            var faker = new Faker();
            return Game.Create(
                Name.Create(faker.Commerce.ProductName()),
                faker.Commerce.ProductDescription(),
                Price.Create(faker.Random.Decimal(1, 100)),
                category
            );
        }

        public static Game BuildWithPrice(decimal price)
        {
            var faker = new Faker();
            return Game.Create(
                Name.Create(faker.Commerce.ProductName()),
                faker.Commerce.ProductDescription(),
                Price.Create(price),
                faker.Commerce.Categories(1)[0]
            );
        }

        public static Game BuildWithAllParameters(string name, string description, decimal price, string category)
        {
            return Game.Create(
                Name.Create(name),
                description,
                Price.Create(price),
                category
            );
        }
    }
}