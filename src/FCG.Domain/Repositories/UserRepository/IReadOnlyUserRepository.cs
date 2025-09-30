using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.UserRepository
{
    public interface IReadOnlyUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    }
}
