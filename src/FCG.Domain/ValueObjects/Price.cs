public sealed record Price
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentException("O preço não pode ser um valor negativo.", nameof(value));
        }

        Value = value;
    }
    public static implicit operator decimal(Price price) => price.Value;

    public Price()
    {
    }
}