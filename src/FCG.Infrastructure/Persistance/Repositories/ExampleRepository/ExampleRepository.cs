using FCG.Domain.Entities;
using FCG.Domain.Repositories.ExampleRepository;

namespace FCG.Infrastructure.Persistance.Repositories.ExampleRepository
{
    public class ExampleRepository : IWriteOnlyExampleRepository
    {
        public readonly FcgDbContext _dbContext;

        public ExampleRepository(FcgDbContext dbContext) => _dbContext = dbContext;

        public async Task AddAsync(Example example)
        {
            await _dbContext.Examples.AddAsync(example);
            await _dbContext.SaveChangesAsync();
        }
    }
}
