using Bogus;
using FCG.Application.UseCases.Authentication.Logout;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Logout
{
    public static class LogoutInputBuilder
    {
        public static LogoutInput Build()
        {
            return new Faker<LogoutInput>()
                .RuleFor(input => input.UserId, faker => faker.Random.Guid())
                .Generate();
        }

        public static LogoutInput BuildWithUserId(Guid userId)
        {
            return new Faker<LogoutInput>()
                .RuleFor(input => input.UserId, userId)
                .Generate();
        }

        public static LogoutInput BuildWithEmptyUserId()
        {
            return new Faker<LogoutInput>()
                .RuleFor(input => input.UserId, Guid.Empty)
                .Generate();
        }
    }
}