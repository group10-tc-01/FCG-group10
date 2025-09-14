using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using FCG.Infrastructure.ContextDb;
using FCG.Domain.EntityUser;
using FCG.Domain.EntityGame;
using Microsoft.EntityFrameworkCore.InMemory; // Importante!

namespace FCG.IntegratedTests
{
    public class FCGDBContextTests
    {
        private DbContextOptions<FCGDbContext> _dbContextOptions;

        public FCGDBContextTests()
        {
            // Usa um banco de dados em memória com um nome único para cada teste
            _dbContextOptions = new DbContextOptionsBuilder<FCGDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Context_Should_AddAndRetrieve_User_Successfully()
        {
            // ARRANGE
            // Cria a entidade user usando o construtor principal
            var user = new EntityUser("Player One", "player.one@email.com", "password123");

            // ACT
            using (var context = new FCGDbContext(_dbContextOptions))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            // ASSERT
            using (var assertContext = new FCGDbContext(_dbContextOptions))
            {
                var savedUser = assertContext.Users.FirstOrDefault(u => u.Id == user.Id);

                Assert.NotNull(savedUser);
                Assert.Equal("Player One", savedUser.Name);
            }
        }

        [Fact]
        public void Context_Should_AddAndRetrieve_Game_Successfully()
        {
            // ARRANGE
            var game = new EntityGame("Super Mario Bros", "Platformer", new DateTime(1985, 9, 13));

            // ACT
            using (var context = new FCGDbContext(_dbContextOptions))
            {
                context.Games.Add(game);
                context.SaveChanges();
            }

            // ASSERT
            using (var assertContext = new FCGDbContext(_dbContextOptions))
            {
                var savedGame = assertContext.Games.FirstOrDefault(g => g.Id == game.Id);

                Assert.NotNull(savedGame);
                Assert.Equal("Super Mario Bros", savedGame.Title);
            }
        }
    }
}