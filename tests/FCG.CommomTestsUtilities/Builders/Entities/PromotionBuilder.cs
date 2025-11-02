using Bogus;
using FCG.Domain.Entities;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public class PromotionBuilder
    {
        public static Promotion Build()
        {
            return new Faker<Promotion>().CustomInstantiator(f => Promotion.Create(f.Random.Guid(), f.Random.Decimal(1, 100), f.Date.Past(2), f.Date.Future(1))).Generate();
        }
    }
}
