using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories.UserRepository
{
    public class UserRepository : IReadOnlyUserRepository, IWriteOnlyUserRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public UserRepository(FcgDbContext context)
        {
            _fcgDbContext = context;
        }

        public Task AddAsync(User user, Wallet wallet)
        {
            _fcgDbContext.Users.Add(user);
            _fcgDbContext.Wallets.Add(wallet);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            _fcgDbContext.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _fcgDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task<(IEnumerable<User> Items, int TotalCount)> GetQueryableAllUsers(
            string? emailFilter,
            string? roleFilter,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            IQueryable<User> query = _fcgDbContext.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(emailFilter))
            {
                var searchEmail = emailFilter.Trim().ToLower();
                query = query.Where(u => u.Email.Value.ToLower().Contains(searchEmail));
            }

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                if (System.Enum.TryParse<Role>(roleFilter, true, out Role filterRole))
                {
                    query = query.Where(u => u.Role == filterRole);
                }
            }

            int totalCount = await query.CountAsync(cancellationToken);

            IEnumerable<User> items = await query
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _fcgDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.IsActive &&
                                    u.Email.Value == email &&
                                    u.Password.Value == password);

            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _fcgDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IsActive && u.Id == id, cancellationToken);

            return user;
        }

        public async Task<User?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _fcgDbContext.Users
                .AsNoTracking()
                .Include(u => u.Wallet)
                .Include(u => u.Library)
                .FirstOrDefaultAsync(u => u.IsActive && u.Id == id, cancellationToken);

            return user;
        }

    }
}