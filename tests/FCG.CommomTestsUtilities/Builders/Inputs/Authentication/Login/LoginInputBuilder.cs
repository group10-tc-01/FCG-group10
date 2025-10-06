using Bogus;
using FCG.Application.UseCases.Authentication.Login;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login
{
    public static class LoginInputBuilder
    {
        public static LoginInput Build()
        {
            return new Faker<LoginInput>().RuleFor(input => input.Email, faker => faker.Internet.Email())
                                           .RuleFor(input => input.Password, faker => faker.Internet.Password())
                                           .Generate();
        }

        public static LoginInput BuildWithEmptyEmail()
        {
            return new Faker<LoginInput>().RuleFor(input => input.Email, string.Empty)
                                           .RuleFor(input => input.Password, faker => faker.Internet.Password())
                                           .Generate();
        }

        public static LoginInput BuildWithInvalidEmail()
        {
            return new Faker<LoginInput>().RuleFor(input => input.Email, faker => faker.Lorem.Word())
                                           .RuleFor(input => input.Password, faker => faker.Internet.Password())
                                           .Generate();
        }

        public static LoginInput BuildWithEmptyPassword()
        {
            return new Faker<LoginInput>().RuleFor(input => input.Email, faker => faker.Internet.Email())
                                           .RuleFor(input => input.Password, string.Empty)
                                           .Generate();
        }

        public static LoginInput BuildWithValues(string email, string password)
        {
            return new Faker<LoginInput>().RuleFor(input => input.Email, email)
                                           .RuleFor(input => input.Password, password)
                                           .Generate();
        }
    }
}
