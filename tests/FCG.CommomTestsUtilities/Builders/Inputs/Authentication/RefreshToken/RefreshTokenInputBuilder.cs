using Bogus;
using FCG.Application.UseCases.Authentication.RefreshToken;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Authentication.RefreshToken
{
    public static class RefreshTokenInputBuilder
    {
        public static RefreshTokenInput Build()
        {
            return new Faker<RefreshTokenInput>()
                .RuleFor(input => input.RefreshToken, faker => faker.Random.AlphaNumeric(32))
                .Generate();
        }

        public static RefreshTokenInput BuildWithToken(string refreshToken)
        {
            return new Faker<RefreshTokenInput>()
                .RuleFor(input => input.RefreshToken, refreshToken)
                .Generate();
        }

        public static RefreshTokenInput BuildWithEmptyToken()
        {
            return new Faker<RefreshTokenInput>()
                .RuleFor(input => input.RefreshToken, string.Empty)
                .Generate();
        }
    }
}