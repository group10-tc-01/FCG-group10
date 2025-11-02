using Bogus;
using FCG.Application.UseCases.Promotions.Create;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Promotions.Create
{
    public static class CreatePromotionInputBuilder
    {
        public static CreatePromotionRequest Build()
        {
            return new Faker<CreatePromotionRequest>()
                .RuleFor(input => input.GameId, faker => Guid.NewGuid())
                .RuleFor(input => input.DiscountPercentage, faker => faker.Random.Decimal(1, 100))
                .RuleFor(input => input.StartDate, faker => DateTime.UtcNow.AddDays(1))
                .RuleFor(input => input.EndDate, faker => DateTime.UtcNow.AddDays(30))
                .Generate();
        }

        public static CreatePromotionRequest BuildWithGameId(Guid gameId)
        {
            return new Faker<CreatePromotionRequest>()
                .RuleFor(input => input.GameId, gameId)
                .RuleFor(input => input.DiscountPercentage, faker => faker.Random.Decimal(1, 100))
                .RuleFor(input => input.StartDate, faker => DateTime.UtcNow.AddDays(1))
                .RuleFor(input => input.EndDate, faker => DateTime.UtcNow.AddDays(30))
                .Generate();
        }

        public static CreatePromotionRequest BuildWithEmptyGameId()
        {
            return new Faker<CreatePromotionRequest>()
                .RuleFor(input => input.GameId, Guid.Empty)
                .RuleFor(input => input.DiscountPercentage, faker => faker.Random.Decimal(1, 100))
                .RuleFor(input => input.StartDate, faker => DateTime.UtcNow.AddDays(1))
                .RuleFor(input => input.EndDate, faker => DateTime.UtcNow.AddDays(30))
                .Generate();
        }

        public static CreatePromotionRequest BuildWithInvalidDiscount(decimal discount)
        {
            return new Faker<CreatePromotionRequest>()
                .RuleFor(input => input.GameId, faker => Guid.NewGuid())
                .RuleFor(input => input.DiscountPercentage, discount)
                .RuleFor(input => input.StartDate, faker => DateTime.UtcNow.AddDays(1))
                .RuleFor(input => input.EndDate, faker => DateTime.UtcNow.AddDays(30))
                .Generate();
        }

        public static CreatePromotionRequest BuildWithPastStartDate()
        {
            return new Faker<CreatePromotionRequest>()
                .RuleFor(input => input.GameId, faker => Guid.NewGuid())
                .RuleFor(input => input.DiscountPercentage, faker => faker.Random.Decimal(1, 100))
                .RuleFor(input => input.StartDate, faker => DateTime.UtcNow.AddDays(-10))
                .RuleFor(input => input.EndDate, faker => DateTime.UtcNow.AddDays(30))
                .Generate();
        }

        public static CreatePromotionRequest BuildWithEndDateBeforeStartDate()
        {
            return new Faker<CreatePromotionRequest>()
                .RuleFor(input => input.GameId, faker => Guid.NewGuid())
                .RuleFor(input => input.DiscountPercentage, faker => faker.Random.Decimal(1, 100))
                .RuleFor(input => input.StartDate, faker => DateTime.UtcNow.AddDays(30))
                .RuleFor(input => input.EndDate, faker => DateTime.UtcNow.AddDays(1))
                .Generate();
        }

        public static CreatePromotionRequest BuildWithDates(DateTime startDate, DateTime endDate)
        {
            return new Faker<CreatePromotionRequest>()
                .RuleFor(input => input.GameId, faker => Guid.NewGuid())
                .RuleFor(input => input.DiscountPercentage, faker => faker.Random.Decimal(1, 100))
                .RuleFor(input => input.StartDate, startDate)
                .RuleFor(input => input.EndDate, endDate)
                .Generate();
        }
    }
}
