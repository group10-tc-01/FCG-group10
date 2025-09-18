using FCG.Domain.Enum;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        private User(Name name, Email email, Password password, Role role)
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }

        public static User Create(Name name, Email email, Password password, Role role)
        {
            return new User(name, email, password, role);
        }

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }
    }
}
