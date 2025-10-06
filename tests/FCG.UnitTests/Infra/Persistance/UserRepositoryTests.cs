using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Infrastructure.Persistance;
using FCG.Infrastructure.Persistance.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FCG.UnitTests.Infrastructure.Persistance
{
    public class UserRepositoryTests : IAsyncLifetime
    {
        private FcgDbContext _context;
        private UserRepository _repository;
        private SqliteConnection _connection;

        private readonly User _testUser;
        private readonly string _testEmail = "testuser@repo.com";
        private readonly string _testPassword = "SecurePassword!1";

        public UserRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var _dbContextOptions = new DbContextOptionsBuilder<FcgDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new FcgDbContext(_dbContextOptions);
            _repository = new UserRepository(_context);

            _testUser = User.Create("Repo Test", _testEmail, _testPassword, Role.User);
        }

        public async Task InitializeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            _context.Users.Add(_testUser);
            await _context.SaveChangesAsync();

            _context.Entry(_testUser).State = EntityState.Detached;
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
            _connection.Close();
        }

        [Fact]
        public async Task GetByEmailAsync_GivenExistingEmail_ShouldReturnUser()
        {
            // Act
            var userFromRepo = await _repository.GetByEmailAsync(_testEmail, CancellationToken.None);

            // Assert
            userFromRepo.Should().NotBeNull();
            userFromRepo.Email.Value.Should().Be(_testEmail);
            userFromRepo.Password.VerifyPassword(_testPassword).Should().BeTrue();
        }

        [Fact]
        public async Task GetByEmailAsync_GivenNonExistingEmail_ShouldReturnNull()
        {
            // Act
            var userFromRepo = await _repository.GetByEmailAsync("nonexistent@test.com", CancellationToken.None);

            // Assert
            userFromRepo.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_GivenNewUser_ShouldPersistUserInDatabase()
        {
            // Arrange
            var newUserEmail = "new@test.com";
            var newUser = User.Create("New User", newUserEmail, "NewSecure!1", Role.User);

            // Act
            await _repository.AddAsync(newUser);
            await _context.SaveChangesAsync();

            // Assert
            var userFromDb = await _context.Users.SingleOrDefaultAsync(u => u.Email == newUserEmail);

            userFromDb.Should().NotBeNull();
            userFromDb.Name.Value.Should().Be("New User");
            userFromDb.Email.Value.Should().Be(newUserEmail);
        }

        [Fact]
        public async Task AnyAdminAsync_ShouldReturnFalse_WhenNoAdminExists()
        {
            // Arrange
            var user = User.Create("User", "user@mail.com", "admin@123", Role.User);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.AnyAdminAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AnyAdminAsync_ShouldReturnTrue_WhenAnyAdminExists()
        {
            // Arrange
            var user = User.Create("User", "user@mail.com", "admin@123", Role.Admin);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.AnyAdminAsync();

            // Assert
            result.Should().BeTrue();
        }
    }
}

