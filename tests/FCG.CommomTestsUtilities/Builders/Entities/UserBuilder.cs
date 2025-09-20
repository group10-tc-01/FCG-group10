using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.ValueObjects;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public class UserBuilder
    {
        // Propriedades do construtor
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }

        public static UserBuilder Build(Role role = Role.Admin)
        {
            // Cria um construtor com dados válidos para teste
            return new UserBuilder
            {
                Name = Name.Create("Rhuan Oliveira"),
                Email = Email.Create("rhuan.oliveira@gmail.com"),
                Password = Password.Create("Password@123"),
                Role = role // Atribuindo a role recebida como parâmetro
            };
        }
    }
}
