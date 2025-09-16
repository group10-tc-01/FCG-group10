namespace FCG.Domain.Entities
{
    public class Example : BaseEntity
    {
        private Example(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public static Example Create(string name, string description)
        {
            return new Example(name, description);
        }

        public string Name { get; private set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}