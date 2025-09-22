using FCG.Domain.Exceptions;

namespace FCG.Domain.ValueObjects
{
    public record Balance
    {
        public decimal Value { get; }

        private Balance(decimal value)
        {
            if (value < 0)
                throw new DomainException("Balance cannot be negative.");

            Value = value;
        }

        public static Balance Create(decimal value)
        {
            return new Balance(value);
        }

        public static implicit operator decimal(Balance balance) => balance.Value;
        public static implicit operator Balance(decimal value) => Create(value);

        public override string ToString() => Value.ToString("F2");
    }
}
