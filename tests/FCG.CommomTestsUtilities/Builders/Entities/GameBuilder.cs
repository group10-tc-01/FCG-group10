public class GameBuilder
{
    public string Name { get; private set; } // Mudança: usar string ao invés de Name
    public string Description { get; private set; }
    public decimal Price { get; private set; } // Mudança: usar decimal ao invés de Price
    public string Category { get; private set; }

    public static GameBuilder Build()
    {
        return new GameBuilder
        {
            Name = "Fifa", // Mudança: usar string diretamente
            Description = "An open-world adventure game.",
            Price = 59.99m, // Mudança: usar decimal diretamente
            Category = "RPG"
        };
    }
}