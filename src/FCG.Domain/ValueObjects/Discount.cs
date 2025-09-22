using FCG.Domain.Exceptions;

namespace FCG.Domain.ValueObjects
{
    public record Discount
    {
        public decimal Value { get; }

        private Discount(decimal value)
        {
            if (value < 0 || value > 100)
                throw new DomainException("Discount must be between 0 and 100.");

            Value = value;
        }

        public static Discount Create(decimal value)
        {

            return new Discount(value);
        }

        public static implicit operator decimal(Discount discount) => discount.Value;
        public static implicit operator Discount(decimal value) => Create(value);

        public override string ToString() => Value.ToString("F2");
    }
}
