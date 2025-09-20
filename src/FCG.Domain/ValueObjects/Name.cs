namespace FCG.Domain.ValueObjects
{
    public record Name
    {
        public string Value { get; }

        private Name(string value)
        {
            Value = value;
        }

        private Name()
        {
        }

        public static Name Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be null or empty.");

            if (value.Length < 2)
                throw new ArgumentException("Name must be at least 2 characters long.");

            if (value.Length > 255)
                throw new ArgumentException("Name cannot exceed 255 characters.");

            if (!IsValidName(value))
                throw new ArgumentException("Name contains invalid characters.");

            return new Name(value.Trim());
        }

        public static implicit operator Name(string value) => Create(value);

        public static implicit operator string(Name name) => name.Value;

        private static bool IsValidName(string name)
        {
            return name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) ||
                                 c == '-' || c == '\'' ||
                                 c == '.' || char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.NonSpacingMark);
        }

        public override string ToString() => Value;
    }
}
