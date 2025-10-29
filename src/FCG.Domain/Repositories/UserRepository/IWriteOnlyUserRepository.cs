using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.UserRepository
{
    public interface IWriteOnlyUserRepository
    {
        Task AddAsync(User user);
    }
}
