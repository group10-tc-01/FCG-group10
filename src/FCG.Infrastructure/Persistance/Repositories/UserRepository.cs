using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IReadOnlyUserRepository, IWriteOnlyUserRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public UserRepository(FcgDbContext context)
        {
            _fcgDbContext = context;
        }

        public async Task AddAsync(User user)
        {
            await _fcgDbContext.Users.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _fcgDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task<(IEnumerable<User> items, int totalCount)> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            IQueryable<User> query = _fcgDbContext.Users.AsNoTracking();

            int totalCount = await query.CountAsync(cancellationToken);

            IEnumerable<User> items = await query
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _fcgDbContext.Users.FirstOrDefaultAsync(u => u.IsActive && u.Id == id, cancellationToken);

            return user;
        }

        public async Task<User?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _fcgDbContext.Users
                .AsNoTracking()
                .Include(u => u.Wallet)
                .Include(u => u.Library)
                .ThenInclude(library => library!.LibraryGames)
                .FirstOrDefaultAsync(u => u.IsActive && u.Id == id, cancellationToken);

            return user;
        }

        public async Task<bool> AnyAdminAsync(CancellationToken cancellationToken = default)
        {
            return await _fcgDbContext.Users.AsNoTracking().AnyAsync(u => u.Role == Role.Admin, cancellationToken);
        }

        public IQueryable<User> GetAllUsersWithFilters(string? name = null, string? email = null)
        {
            var query = _fcgDbContext.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(u => u.Name.Value.Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(u => u.Email.Value.Contains(email));

            return query;
        }
    }
}