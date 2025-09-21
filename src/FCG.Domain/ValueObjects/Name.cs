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

            var trimmedValue = value.Trim();

            if (trimmedValue.Length < 2)
                throw new ArgumentException("Name must be at least 2 characters long.");

            if (trimmedValue.Length > 255)
                throw new ArgumentException("Name cannot exceed 255 characters.");

            return new Name(trimmedValue);
        }

        public static implicit operator Name(string value) => Create(value);

        public static implicit operator string(Name name) => name.Value;

        public override string ToString() => Value;
    }
}