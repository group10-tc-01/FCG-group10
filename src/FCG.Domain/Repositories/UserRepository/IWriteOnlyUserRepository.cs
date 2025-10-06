namespace FCG.Domain.Repositories.UserRepository
{
    public interface IWriteOnlyUserRepository
    {
        Task AddAsync(Entities.User user);
        Task UpdateAsync(Entities.User user);
    }
}
