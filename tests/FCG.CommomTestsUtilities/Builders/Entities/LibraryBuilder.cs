using Bogus;
using FCG.Domain.Entities;

namespace FCG.CommomTestsUtilities.Builders.Entities
{
    public static class LibraryBuilder
    {
        public static Library Build()
        {
            return new Faker<Library>().CustomInstantiator(f => Library.Create(Guid.NewGuid()));
        }
    }
}