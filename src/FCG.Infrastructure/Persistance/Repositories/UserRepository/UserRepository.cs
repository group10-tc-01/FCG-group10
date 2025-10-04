using FCG.Domain.Entities;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IReadOnlyUserRepository, IWriteOnlyUserRepository
    {
        private readonly FcgDbContext _context;

        public UserRepository(FcgDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(User user)
        {
            _context.Users.Add(user);
            return Task.CompletedTask;
        }
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var emailObject = Email.Create(email);
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == emailObject, cancellationToken);
        }

        public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}