using Bogus;
using FCG.Domain.Entities;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public static class RefreshTokenBuilder
    {
        public static RefreshToken Build()
        {
            return new Faker<RefreshToken>().CustomInstantiator(f => RefreshToken.Create(f.Random.AlphaNumeric(30), Guid.NewGuid(), TimeSpan.FromDays(7))).Generate();
        }

        public static RefreshToken BuildExpired()
        {
            return new Faker<RefreshToken>().CustomInstantiator(f => RefreshToken.Create(f.Random.AlphaNumeric(30), Guid.NewGuid(), TimeSpan.FromSeconds(-1))).Generate();
        }

        public static RefreshToken BuildWithUserId(Guid userId)
        {
            return new Faker<RefreshToken>().CustomInstantiator(f => RefreshToken.Create(f.Random.AlphaNumeric(30), userId, TimeSpan.FromDays(7))).Generate();
        }
    }
}
