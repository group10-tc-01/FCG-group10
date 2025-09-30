using FCG.Domain.Enum;
using FCG.Domain.Events.User;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }

        public Library? Library { get; }
        public Wallet? Wallet { get; }

        private User(Name name, Email email, Password password, Role role)
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }

        public static User Create(string name, string email, string password, Role role)
        {
            var user = new User(name, email, password, role);
            user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Name, user.Email));
            return user;
        }
    }
}