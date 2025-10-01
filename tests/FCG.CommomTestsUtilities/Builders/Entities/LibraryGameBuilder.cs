using Bogus;
using FCG.Domain.Entities;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public class LibraryGameBuilder
    {
        public static LibraryGame Build()
        {
            return new Faker<LibraryGame>().CustomInstantiator(f => LibraryGame.Create(f.Random.Guid(), f.Random.Guid(), f.Random.Decimal(1, 100))).Generate();
        }
    }
}
