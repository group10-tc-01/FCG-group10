using FCG.Domain.Entities;
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

        public Task AddAsync(User user)
        {
            _fcgDbContext.Users.Add(user);
            return Task.CompletedTask;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _fcgDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _fcgDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.IsActive &&
                                    u.Email.Value == email &&
                                    u.Password.Value == password);

            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user = await _fcgDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.IsActive && u.Id == id);

            return user;
        }
    }
}