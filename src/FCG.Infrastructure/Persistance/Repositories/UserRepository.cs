using FCG.Domain.Entities;
using FCG.Domain.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IReadOnlyUserRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public UserRepository(FcgDbContext fcgDbContext) => _fcgDbContext = fcgDbContext;

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
