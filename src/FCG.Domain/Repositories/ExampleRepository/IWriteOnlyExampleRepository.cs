using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.ExampleRepository
{
    public interface IWriteOnlyExampleRepository
    {
        Task AddAsync(Example example);
    }
}
