using FCG.Domain.Entities;
using FCG.Domain.Repositories.LibraryRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class LibraryRepository : IWriteOnlyLibraryRepository, IReadOnlyLibraryRepository
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

        public async Task<Library?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _fcgDbContext.Libraries.FirstOrDefaultAsync(l => l.UserId == userId, cancellationToken);
        }

        public async Task<Library?> GetByUserIdWithGamesAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _fcgDbContext.Libraries
                .Include(l => l.LibraryGames)
                .ThenInclude(lg => lg.Game)
                .FirstOrDefaultAsync(l => l.UserId == userId, cancellationToken);
        }
    }
}
