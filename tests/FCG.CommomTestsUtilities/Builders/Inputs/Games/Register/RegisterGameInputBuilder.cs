using Bogus;
using FCG.Application.UseCases.Games.Register;
using FCG.Domain.Enum;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Games.Register
{
    public static class RegisterGameInputBuilder
    {
        public static RegisterGameInput Build()
        {
            return new Faker<RegisterGameInput>()
                .RuleFor(input => input.Name, faker => faker.Commerce.ProductName())
                .RuleFor(input => input.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(input => input.Price, faker => faker.Random.Decimal(1, 1000))
                .RuleFor(input => input.Category, faker => faker.PickRandom<GameCategory>())
                .Generate();
        }

        public static RegisterGameInput BuildWithEmptyName()
        {
            return new Faker<RegisterGameInput>()
                .RuleFor(input => input.Name, string.Empty)
                .RuleFor(input => input.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(input => input.Price, faker => faker.Random.Decimal(1, 1000))
                .RuleFor(input => input.Category, faker => faker.PickRandom<GameCategory>())
                .Generate();
        }

        public static RegisterGameInput BuildWithEmptyDescription()
        {
            return new Faker<RegisterGameInput>()
                .RuleFor(input => input.Name, faker => faker.Commerce.ProductName())
                .RuleFor(input => input.Description, string.Empty)
                .RuleFor(input => input.Price, faker => faker.Random.Decimal(1, 1000))
                .RuleFor(input => input.Category, faker => faker.PickRandom<GameCategory>())
                .Generate();
        }

        public static RegisterGameInput BuildWithZeroPrice()
        {
            return new Faker<RegisterGameInput>()
                .RuleFor(input => input.Name, faker => faker.Commerce.ProductName())
                .RuleFor(input => input.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(input => input.Price, 0)
                .RuleFor(input => input.Category, faker => faker.PickRandom<GameCategory>())
                .Generate();
        }

        public static RegisterGameInput BuildWithNegativePrice()
        {
            return new Faker<RegisterGameInput>()
                .RuleFor(input => input.Name, faker => faker.Commerce.ProductName())
                .RuleFor(input => input.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(input => input.Price, faker => faker.Random.Decimal(-100, -1))
                .RuleFor(input => input.Category, faker => faker.PickRandom<GameCategory>())
                .Generate();
        }

        public static RegisterGameInput BuildWithInvalidCategory()
        {
            return new Faker<RegisterGameInput>()
                .RuleFor(input => input.Name, faker => faker.Commerce.ProductName())
                .RuleFor(input => input.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(input => input.Price, faker => faker.Random.Decimal(1, 1000))
                .RuleFor(input => input.Category, (GameCategory)999) // Invalid enum value
                .Generate();
        }

        public static RegisterGameInput BuildWithLongName()
        {
            return new Faker<RegisterGameInput>()
                .RuleFor(input => input.Name, faker => faker.Lorem.Letter(256))
                .RuleFor(input => input.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(input => input.Price, faker => faker.Random.Decimal(1, 1000))
                .RuleFor(input => input.Category, faker => faker.PickRandom<GameCategory>())
                .Generate();
        }

        public static RegisterGameInput BuildWithLongDescription()
        {
            return new Faker<RegisterGameInput>()
                .RuleFor(input => input.Name, faker => faker.Commerce.ProductName())
                .RuleFor(input => input.Description, faker => faker.Lorem.Letter(501))
                .RuleFor(input => input.Price, faker => faker.Random.Decimal(1, 1000))
                .RuleFor(input => input.Category, faker => faker.PickRandom<GameCategory>())
                .Generate();
        }

        public static RegisterGameInput BuildWithName(string name)
        {
            return new Faker<RegisterGameInput>()
                .UseSeed(12345)
                .RuleFor(input => input.Name, name)
                .RuleFor(input => input.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(input => input.Price, faker => faker.Random.Decimal(1, 1000))
                .RuleFor(input => input.Category, faker => faker.PickRandom<GameCategory>())
                .Generate();
        }

        public static RegisterGameInput BuildWithNameAndCategory(string name, GameCategory category)
        {
            return new RegisterGameInput
            {
                Name = name,
                Category = category,
                Description = "Test game description",
                Price = 49.99m
            };
        }

        public static RegisterGameInput BuildWithNameAndPrice(string name, decimal price)
        {
            return new RegisterGameInput
            {
                Name = name,
                Category = GameCategory.Action,
                Description = "Test game description",
                Price = price
            };
        }

        public static RegisterGameInput BuildWithDetails(string name, GameCategory category, decimal price)
        {
            return new RegisterGameInput
            {
                Name = name,
                Category = category,
                Description = "Test game description",
                Price = price
            };
        }
    }
}