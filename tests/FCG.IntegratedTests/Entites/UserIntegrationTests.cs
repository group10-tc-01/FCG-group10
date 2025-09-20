using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Infrastructure.Persistance;
using FCG.IntegratedTests.Configurations; // Adicione esta linha
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class UserIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public UserIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Given_UserWithWalletAndLibrary_When_SavedToDb_Then_ShouldBePersistedSuccessfully()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

        var user = User.Create(
            "Test User",
            $"{Guid.NewGuid()}@example.com",
            "P@ssw0rd!23",
            Role.Admin
        );


        context.Users.Add(user);
        await context.SaveChangesAsync();


        var userFromDb = await context.Users
            .Include(u => u.Wallet)
            .Include(u => u.Library)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        userFromDb.Should().NotBeNull();
        userFromDb.Name.Value.Should().Be("Test User");
        userFromDb.Wallet.Should().NotBeNull();
        userFromDb.Library.Should().NotBeNull();
        userFromDb.Wallet.UserId.Should().Be(user.Id);
        userFromDb.Library.UserId.Should().Be(user.Id);
    }


}