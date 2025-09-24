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
            var user = await _fcgDbContext.Users
                .FirstOrDefaultAsync(u => u.IsActive && u.Email.Equals(email) && u.Password.Equals(password));

            return user;
        }
    }
}
