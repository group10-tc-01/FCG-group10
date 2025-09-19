using System;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using FCG.Infrastructure.Persistance.Configuration;
using FluentAssertions;

public class EntityRelationshipsTests : IDisposable
{
    private readonly AppDbContext _context;

    public EntityRelationshipsTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        _context = new AppDbContext(options);

        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }

    [Fact]
    public void RelacionamentoManytoMany_LibraryGame_DeveSerConfiguradoCorretamente()
    {
        var userEmail = new Email("rhuan@example.com");
        var userPassword = new Password("Senha123Segura");
        var gamePrice1 = new Price(199.99m);
        var gamePrice2 = new Price(99.99m);

        var user = new User("Rhuan Marques", userEmail, userPassword, "Admin");
        var library = new Library(user.Id);
        var game1 = new Game("Cyberpunk 2077", "A futuristic RPG game.", gamePrice1, "RPG");
        var game2 = new Game("The Witcher 3: Wild Hunt", "An epic fantasy RPG.", gamePrice2, "RPG");

        _context.Users.Add(user);
        _context.Libraries.Add(library);
        _context.Games.Add(game1);
        _context.Games.Add(game2);
        _context.SaveChanges();

        var libraryGame1 = new LibraryGame(library.Id, game1.Id, gamePrice1.Value);
        var libraryGame2 = new LibraryGame(library.Id, game2.Id, gamePrice2.Value);


        _context.LibraryGames.Add(libraryGame1);
        _context.LibraryGames.Add(libraryGame2);
        _context.SaveChanges();


        var userFromDb = _context.Users
            .Include(u => u.Library)
                .ThenInclude(l => l.LibraryGames)
                    .ThenInclude(lg => lg.Game)
            .FirstOrDefault(u => u.Id == user.Id);


        Assert.NotNull(userFromDb);
        Assert.NotNull(userFromDb.Library);
        Assert.Equal(user.Id, userFromDb.Library.UserId);
        Assert.Equal(2, userFromDb.Library.LibraryGames.Count);

        var gamesInLibrary = userFromDb.Library.LibraryGames.Select(lg => lg.Game).ToList();
        Assert.Contains(gamesInLibrary, g => g.Name == "Cyberpunk 2077");
        Assert.Contains(gamesInLibrary, g => g.Name == "The Witcher 3: Wild Hunt");
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WhenNameIsEmptyOrNull_ShouldThrowException(string name)
    {

        var email = new Email("test@example.com");
        var password = new Password("Password123");
        var role = "User";

        Action act = () => new User(name, email, password, role);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Verifique os dados.");
    }

    [Fact]
    public void Constructor_WhenRoleIsInvalid_ShouldThrowException()
    {
        var name = "John Doe";
        var email = new Email("test@example.com");
        var password = new Password("Password123");
        var role = "";

        Action act = () => new User(name, email, password, role);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Verifique os dados.");
    }
    public class GameTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WhenNameIsEmptyOrNull_ShouldThrowException(string name)
        {
            var price = new Price(59.99m);
            var description = "Description for a game.";
            var category = "Action";

            Action act = () => new Game(name, description, price, category);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Nome obrigatório!");
        }

        [Fact]
        public void Constructor_WhenNameIsLessThan3Characters_ShouldThrowException()
        {
            var name = "ab";
            var price = new Price(59.99m);
            var description = "Description for a game.";
            var category = "Action";

            Action act = () => new Game(name, description, price, category);
            act.Should().Throw<ArgumentException>()
                .WithMessage("O nome deve ter pelo menos 3 caracteres.");
        }

        [Fact]
        public void Constructor_WhenInputIsValid_ShouldNotThrowException()
        {
            var name = "ValidName";
            var price = new Price(59.99m);
            var description = "Description for a game.";
            var category = "Action";

            Action act = () => new Game(name, description, price, category);

            act.Should().NotThrow<ArgumentException>();
        }
    }
}