using Bogus;
using FCG.Domain.Entities;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public class ExampleBuilder
    {
        public static Example Build()
        {
            return new Faker<Example>().CustomInstantiator(f => Example.Create(f.Name.FullName(), f.Commerce.ProductDescription())).Generate();
        }
    }
}
