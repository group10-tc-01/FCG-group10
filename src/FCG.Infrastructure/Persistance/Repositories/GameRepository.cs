using FCG.Domain.Entities;
using FCG.Domain.Enum;
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

        public IQueryable<Game?> GetAllWithFilters(string? name = null, GameCategory? category = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var query = _fcgDbContext.Games
                .Include(g => g!.Promotions)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(g => g!.Name.Value.Contains(name));

            if (category.HasValue)
                query = query.Where(g => g!.Category == category.Value);

            if (minPrice.HasValue)
                query = query.Where(g => g!.Price.Value >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(g => g!.Price.Value <= maxPrice.Value);

            return query;
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
