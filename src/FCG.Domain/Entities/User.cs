using FCG.Domain.Enum;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public Name Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public Password Password { get; private set; } = null!;
        public Role Role { get; private set; }

        public Library? Library { get; }
        public Wallet? Wallet { get; }
        public ICollection<RefreshToken>? RefreshTokens { get; }

        private User(Name name, Email email, Password password, Role role)
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }

        private User() { }

        public static User Create(string name, string email, string password, Role role)
        {
            return new User(name, email, password, role);
        }
    }
}