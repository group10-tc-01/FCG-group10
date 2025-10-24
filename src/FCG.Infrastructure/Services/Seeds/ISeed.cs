namespace FCG.Infrastructure.Services.Seeds
{
    public interface ISeed
    {
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}
