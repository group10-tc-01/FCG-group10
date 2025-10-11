using Bogus;
using FCG.Domain.Entities;
using FCG.Domain.Enum;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public static class UserBuilder
    {
        public static User Build()
        {
            return new Faker<User>().CustomInstantiator(f => User.Create(f.Name.ToString()!, f.Internet.Email(), GenerateValidPassword(f), f.PickRandom<Role>())).Generate();
        }

        public static User BuildAdmin()
        {
            return new Faker<User>().CustomInstantiator(f => User.Create(f.Name.ToString()!, f.Internet.Email(), GenerateValidPassword(f), Role.Admin)).Generate();
        }

        public static User BuildRegularUser()
        {
            return new Faker<User>().CustomInstantiator(f => User.Create(f.Name.ToString()!, f.Internet.Email(), GenerateValidPassword(f), Role.User)).Generate();
        }

        private static string GenerateValidPassword(Faker faker)
        {
            var letter = faker.Random.Char('a', 'z');
            var digit = faker.Random.Char('0', '9');
            var special = faker.PickRandom('!', '@', '#', '$', '%', '^', '&', '*');

            var additionalChars = faker.Random.String2(5, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*");

            var passwordChars = new[] { letter, digit, special }.Concat(additionalChars.ToCharArray()).ToArray();

            for (int i = passwordChars.Length - 1; i > 0; i--)
            {
                int j = faker.Random.Int(0, i);
                (passwordChars[i], passwordChars[j]) = (passwordChars[j], passwordChars[i]);
            }

            return new string(passwordChars);
        }
    }
}
