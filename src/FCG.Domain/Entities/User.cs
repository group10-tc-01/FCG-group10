using FCG.Domain.Enum;
using FCG.Domain.Events.User;
using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;

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
            var user = new User(name, email, password, role);
            user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Name, user.Email));
            return user;
        }

        public void Update(string password)
        {
            Password = Password.CreateFromHash(password);
            UpdatedAt = DateTime.UtcNow;
        }

        public void PromoteToAdmin()
        {
            if (Role == Role.Admin)
                throw new DomainException(ResourceMessages.UserAlreadyAdmin);

            Role = Role.Admin;
        }

        public void DemoteToUser()
        {
            if (Role == Role.User)
                throw new DomainException(ResourceMessages.UserAlreadyUser);

            Role = Role.User;
        }
    }
}