namespace FCG.Domain.Repositories.UserRepository
{
    public interface IWriteOnlyUserRepository
    {
        Task AddAsync(Entities.User user, Entities.Wallet wallet);
        Task UpdateAsync(Entities.User user);
    }
}
