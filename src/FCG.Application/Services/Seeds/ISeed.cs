namespace FCG.Application.Services.Seeds
{
    public interface ISeed
    {
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}
