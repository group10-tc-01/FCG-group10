using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using FCG.Infrastructure.Persistance;
using FCG.IntegratedTests.Configurations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class GameIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GameIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Given_Game_When_SavedToDb_Then_ItShouldBePersistedSuccessfully()
    {
        // ARRANGE
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

        var game = Game.Create(
            Name.Create("Test Game"),
            "A simple test game.",
            Price.Create(19.99m),
            "Action"
        );

        // ACT
        context.Games.Add(game);
        await context.SaveChangesAsync();

        // ASSERT
        var gameFromDb = await context.Games.FindAsync(game.Id);
        gameFromDb.Should().NotBeNull();
        gameFromDb.Name.Value.Should().Be("Test Game");
        gameFromDb.Description.Should().Be("A simple test game.");
        gameFromDb.Price.Value.Should().Be(19.99m);
    }

    [Fact]
    public async Task Given_ExistingGame_When_Deleted_Then_ItShouldNotBeFound()
    {
        // ARRANGE
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

        var gameToDelete = Game.Create(
            Name.Create("Game to Delete"),
            "This game will be deleted.",
            Price.Create(9.99m),
            "Puzzle"
        );
        context.Games.Add(gameToDelete);
        await context.SaveChangesAsync();

        // ACT
        context.Games.Remove(gameToDelete);
        await context.SaveChangesAsync();

        // ASSERT
        var deletedGameFromDb = await context.Games.FindAsync(gameToDelete.Id);
        deletedGameFromDb.Should().BeNull();
    }
}