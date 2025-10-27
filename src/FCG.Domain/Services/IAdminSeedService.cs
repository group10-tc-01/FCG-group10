namespace FCG.Domain.Services
{
    public interface IAdminSeedService
    {
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}
