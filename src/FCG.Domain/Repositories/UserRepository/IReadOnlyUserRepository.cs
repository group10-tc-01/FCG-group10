using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.UserRepository
{
    public interface IReadOnlyUserRepository
    {
        Task<User?> GetByEmailAndPasswordAsync(string email, string password);
    }
}
