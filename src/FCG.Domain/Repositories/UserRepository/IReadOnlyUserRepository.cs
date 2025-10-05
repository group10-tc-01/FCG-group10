using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.UserRepository
{
    public interface IReadOnlyUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
<<<<<<< HEAD
        Task<IQueryable<User>> GetQueryableAllUsers();
=======

        Task<User?> GetByEmailAndPasswordAsync(string email, string password);
        Task<User?> GetByIdAsync(Guid id);
>>>>>>> develop
    }
}
