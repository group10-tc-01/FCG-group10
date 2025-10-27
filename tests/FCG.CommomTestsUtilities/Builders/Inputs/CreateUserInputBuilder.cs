using Bogus;
using FCG.Application.UseCases.Users.Register;

namespace FCG.CommomTestsUtilities.Builders.Inputs
{
    public class CreateUserInputBuilder
    {

        public static RegisterUserRequest Build()
        {
            return new Faker<RegisterUserRequest>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password(8, false, "", "Aa1@"))
                .Generate();
        }


        public static RegisterUserRequest BuildWithInvalidPassword()
        {
            return new Faker<RegisterUserRequest>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => "123") // Senha inválida
                .Generate();
        }


        public static RegisterUserRequest BuildWithWeakPassword()
        {
            return new Faker<RegisterUserRequest>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Lorem.Word() + "123") // Senha sem caracteres especiais
                .Generate();
        }


        public static RegisterUserRequest BuildWithInvalidEmail()
        {
            return new Faker<RegisterUserRequest>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => "invalid-email") // Email inválido
                .RuleFor(u => u.Password, f => f.Internet.Password(8, false, "", "Aa1@"))
                .Generate();
        }

        public static RegisterUserRequest BuildWithEmail(string email)
        {
            return new Faker<RegisterUserRequest>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => email)
                .RuleFor(u => u.Password, f => f.Internet.Password(8, false, "", "Aa1@"))
                .Generate();
        }
    }
}