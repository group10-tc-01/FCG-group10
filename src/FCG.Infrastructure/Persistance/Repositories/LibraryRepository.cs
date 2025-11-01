using FCG.Domain.Entities;
using FCG.Domain.Repositories.LibraryRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class LibraryRepository : IReadOnlyLibraryRepository, IWriteOnlyLibraryRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public LibraryRepository(FcgDbContext context)
        {
            _fcgDbContext = context;
        }
        public async Task<Library?> GetByUserIdAsync(Guid userId)
        {
            return await _fcgDbContext.Libraries
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.UserId == userId);
        }
        public async Task AddAsync(Library library)
        {
            await _fcgDbContext.Libraries.AddAsync(library);
        }
    }
}
