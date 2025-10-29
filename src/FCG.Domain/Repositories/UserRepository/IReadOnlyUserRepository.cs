using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.UserRepository
{
    public interface IReadOnlyUserRepository
    {
        Task<bool> AnyAdminAsync(CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<(IEnumerable<User> items, int totalCount)> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
