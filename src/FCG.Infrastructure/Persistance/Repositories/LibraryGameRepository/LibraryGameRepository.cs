using FCG.Domain.Entities;
using FCG.Domain.Repositories.LibraryGameRespository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories.LibraryGameRepository
{
    public class LibraryGameRepository : IReadOnlyLibraryGameRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public LibraryGameRepository(FcgDbContext fcgDbContext)
        {
            _fcgDbContext = fcgDbContext;
        }
        public async Task<IEnumerable<LibraryGame>> GetLibraryGamesByUserIdAsync(Guid userId)
        {
            var library = await _fcgDbContext.Libraries
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.UserId == userId);

            if (library == null)
            {

                return new List<LibraryGame>();
            }

            return await _fcgDbContext.LibraryGames
                .AsNoTracking()
                .Include(lg => lg.Game)
                .Where(lg => lg.LibraryId == library.Id)
                .ToListAsync();
        }
    }
}
