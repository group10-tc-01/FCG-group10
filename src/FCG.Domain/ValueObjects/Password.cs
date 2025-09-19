namespace FCG.Domain.ValueObjects
{
    public sealed record Password
    {
        private const int MinimumLength = 8;
        public string Value { get; private set; }

        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("A senha não pode ser nula ou vazia.", nameof(value));
            }

            if (value.Length < MinimumLength)
            {
                throw new ArgumentException($"A senha deve ter no mínimo {MinimumLength} caracteres.", nameof(value));
            }

            if (!value.Any(char.IsLetter))
            {
                throw new ArgumentException("A senha deve conter pelo menos uma letra.");
            }

            if (!value.Any(char.IsDigit))
            {
                throw new ArgumentException("A senha deve conter pelo menos um número.");
            }

            if (!value.Any(char.IsUpper))
            {
                throw new ArgumentException("A senha deve conter pelo menos uma letra maiúscula.");
            }

            Value = value;
        }


        public Password() { }
    }
}