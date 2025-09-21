using FCG.Domain.Enum;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }

        public Library Library { get; private set; }
        public Wallet Wallet { get; private set; }

        public User()
        {
        }

        private User(Name name, Email email, Password password, Role role, Library library, Wallet wallet)
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            Library = library;
            Wallet = wallet;
        }

        public static User Create(string name, string email, string password, Role role)
        {
            var nameObject = Name.Create(name);
            var emailObject = Email.Create(email);
            var passwordObject = Password.Create(password);

            // Cria o usuário com as associações corretas
            var user = new User(nameObject, emailObject, passwordObject, role, null, null);

            // A carteira e a biblioteca são entidades filhas e devem ser associadas ao usuário pai
            user.SetWallet(Wallet.Create(user.Id, 1));
            user.SetLibrary(Library.Create(user.Id));

            return user;
        }

        public void SetWallet(Wallet wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }

            Wallet = wallet;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetLibrary(Library library)
        {
            if (library == null)
            {
                throw new ArgumentNullException(nameof(library));
            }

            Library = library;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}