using FCG.Domain.Entities;
using FCG.Domain.Repositories.LibraryRepository;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class LibraryRepository : IWriteOnlyLibraryRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public LibraryRepository(FcgDbContext context)
        {
            _fcgDbContext = context;
        }

        public async Task AddAsync(Library library)
        {
            await _fcgDbContext.Libraries.AddAsync(library);
        }
    }
}
