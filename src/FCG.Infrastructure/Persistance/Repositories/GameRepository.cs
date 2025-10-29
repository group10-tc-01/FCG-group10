using FCG.Domain.Entities;
using FCG.Domain.Repositories.GamesRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class GameRepository : IWriteOnlyGameRepository, IReadOnlyGameRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public GameRepository(FcgDbContext context)
        {
            _fcgDbContext = context;
        }

        public async Task AddAsync(Game game)
        {
            await _fcgDbContext.Games.AddAsync(game);
        }

        public IQueryable<Game?> GetAllAsQueryable()
        {
            var games = _fcgDbContext.Games.AsNoTracking().AsQueryable();

            return games;
        }

        public async Task<Game?> GetByNameAsync(string name)
        {
            var game = await _fcgDbContext.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Name.Value == name);

            return game;
        }

        public async Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _fcgDbContext.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _fcgDbContext.Games
                .AsNoTracking()
                .AnyAsync(g => g.Id == id, cancellationToken);
        }
    }
}
