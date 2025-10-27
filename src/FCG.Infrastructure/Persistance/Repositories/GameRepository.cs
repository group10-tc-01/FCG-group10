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

        public async Task<Game?> GetByNameAsync(string name)
        {
            var game = await _fcgDbContext.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Name.Value == name);

            return game;
        }
    }
}
