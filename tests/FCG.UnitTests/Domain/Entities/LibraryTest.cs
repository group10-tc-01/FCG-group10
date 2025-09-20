using FCG.Domain.Entities;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class LibraryTests
    {
        [Fact]
        public void Given_ValidUserId_When_CreateLibrary_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var library = Library.Create(userId);

            // Assert
            library.Should().NotBeNull();
            library.Id.Should().NotBe(Guid.Empty);
            library.UserId.Should().Be(userId);
            library.LibraryGames.Should().NotBeNull();
        }

        [Fact]
        public void Given_EmptyUserId_When_CreateLibrary_Then_ShouldCreateWithEmptyId()
        {
            // Arrange
            var emptyUserId = Guid.Empty;

            // Act
            var library = Library.Create(emptyUserId);

            // Assert
            library.Should().NotBeNull();
            library.UserId.Should().Be(Guid.Empty);
        }

        [Fact]
        public void Given_ValidUserId_When_CreateLibraryUsingConstructor_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var library = new Library(userId);

            // Assert
            library.Should().NotBeNull();
            library.UserId.Should().Be(userId);
        }

        [Fact]
        public void Given_NewLibrary_When_CheckLibraryGamesCollection_Then_ShouldBeInitialized()
        {
            // Arrange & Act
            var library = Library.Create(Guid.NewGuid());

            // Assert
            library.LibraryGames.Should().NotBeNull();
        }

        [Fact]
        public void Given_TwoLibrariesWithSameUserId_When_Compare_Then_ShouldHaveSameUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var library1 = Library.Create(userId);
            var library2 = Library.Create(userId);

            // Assert
            library1.UserId.Should().Be(library2.UserId);
        }

        [Fact]
        public void Given_Library_When_SetUser_Then_ShouldSetUserProperty()
        {

            var library = Library.Create(Guid.NewGuid());
            var user = new User();


            library.User = user;


            library.User.Should().Be(user);
        }

    }
}

