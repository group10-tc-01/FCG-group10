using FCG.Domain.Exceptions;

namespace FCG.Domain.ValueObjects
{
    public record Password
    {
        public string Value { get; }

        private Password(string value)
        {
            Value = value;
        }

        public static Password Create(string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
                throw new DomainException("Password cannot be null or empty.");

            if (plainTextPassword.Length < 8)
                throw new DomainException("Password must be at least 8 characters long.");

            if (!ContainsLetter(plainTextPassword))
                throw new DomainException("Password must contain at least one letter.");

            if (!ContainsDigit(plainTextPassword))
                throw new DomainException("Password must contain at least one number.");

            if (!ContainsSpecialCharacter(plainTextPassword))
                throw new DomainException("Password must contain at least one special character.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
            return new Password(hashedPassword);
        }

        public static Password FromHash(string hashedValue)
        {
            if (string.IsNullOrWhiteSpace(hashedValue))
                throw new DomainException("Hashed password cannot be null or empty.");

            return new Password(hashedValue);
        }

        public bool VerifyPassword(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, this.Value);
        }
        private static bool ContainsLetter(string password) => password.Any(char.IsLetter);

        private static bool ContainsDigit(string password) => password.Any(char.IsDigit);

        private static bool ContainsSpecialCharacter(string password) => password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c));

        public static implicit operator string(Password password) => password.Value;
        public static implicit operator Password(string value) => Create(value);

        public override string ToString() => Value;
    }
}


