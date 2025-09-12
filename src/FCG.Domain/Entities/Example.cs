namespace FCG.Domain.Entities
{
    public class Example : BaseEntity
    {
        private Example(string name)
        {
            Name = name;
        }

        public static Example Create(string name)
        {
            return new Example(name);
        }

        public string Name { get; private set; } = string.Empty;
    }
}