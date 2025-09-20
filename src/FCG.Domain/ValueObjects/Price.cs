public sealed record Price
{
    public decimal Value { get; }

    private Price(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentException("The price cannot be a negative value.", nameof(value));
        }
        Value = value;
    }

    private Price()
    {
    }

    public static Price Create(decimal value)
    {
        return new Price(value);
    }
    public static implicit operator decimal(Price price) => price.Value;
    public static implicit operator Price(decimal value) => Create(value);

    public override string ToString() => Value.ToString("F2");

}