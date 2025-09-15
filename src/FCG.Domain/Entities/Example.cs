using System.Diagnostics.CodeAnalysis;

namespace FCG.Domain.Entities
{
    [ExcludeFromCodeCoverage(Justification = "Example code, will be removed")]
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