using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.UserRepository
{
    public interface IReadOnlyUserRepository
    {
        Task<bool> AnyAdminAsync(CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<User?> GetByEmailAndPasswordAsync(string email, string password);
        Task<User?> GetByIdAsync(Guid id);
    }
}
