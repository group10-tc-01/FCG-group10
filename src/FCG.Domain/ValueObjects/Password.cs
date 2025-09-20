namespace FCG.Domain.ValueObjects
{
    public record Password
    {
        public string Value { get; }

        private Password(string value)
        {
            Value = value;
        }
        private Password()
        {
        }

        public static Password Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Password cannot be null or empty.");

            if (value.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            if (!ContainsLetter(value))
                throw new ArgumentException("Password must contain at least one letter.");

            if (!ContainsDigit(value))
                throw new ArgumentException("Password must contain at least one number.");

            if (!ContainsSpecialCharacter(value))
                throw new ArgumentException("Password must contain at least one special character.");

            return new Password(value);
        }

        private static bool ContainsLetter(string password) => password.Any(char.IsLetter);

        private static bool ContainsDigit(string password) => password.Any(char.IsDigit);

        private static bool ContainsSpecialCharacter(string password) =>
            password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c));

        public static implicit operator string(Password password) => password.Value;
        public static implicit operator Password(string value) => Create(value);

        public override string ToString() => Value;
    }
}