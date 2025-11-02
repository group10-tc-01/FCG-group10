using FCG.Application.UseCases.Library.GetMyLibrary;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Requests;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Services;
using FCG.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Library.GetMyLibrary
{
    public class GetMyLibraryUseCaseTest
    {
        private static GetMyLibraryUseCase BuildUseCase(
            User? user = null,
            Domain.Entities.Library? library = null)
        {
            user ??= UserBuilder.Build();
            library ??= LibraryBuilder.Build(user.Id);

            var loggedUserMock = new Mock<ILoggedUser>();
            loggedUserMock
                .Setup(x => x.GetLoggedUserAsync())
                .ReturnsAsync(user);

            var readOnlyLibraryRepoMock = new Mock<IReadOnlyLibraryRepository>();
            readOnlyLibraryRepoMock
                .Setup(x => x.GetByUserIdWithGamesAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(library);

            var loggerMock = new Mock<ILogger<GetMyLibraryUseCase>>();

            return new GetMyLibraryUseCase(
                loggedUserMock.Object,
                readOnlyLibraryRepoMock.Object,
                loggerMock.Object);
        }

        [Fact]
        public async Task Given_ValidLoggedUser_When_GettingLibrary_Then_ShouldReturnLibraryWithGames()
        {
            // Arrange
            var user = UserBuilder.Build();
            var library = LibraryBuilder.Build(user.Id);
            
            var game1 = Game.Create(
                Name.Create("Game 1"),
                "Description 1",
                Price.Create(50.00m),
                GameCategory.Action);
            
            var game2 = Game.Create(
                Name.Create("Game 2"),
                "Description 2",
                Price.Create(80.00m),
                GameCategory.Adventure);

            library.AddGame(game1.Id, Price.Create(45.00m));
            library.AddGame(game2.Id, Price.Create(75.00m));

            var useCase = BuildUseCase(user, library);
            var request = GetMyLibraryInputBuilder.Build();

            // Act
            var response = await useCase.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.LibraryId.Should().Be(library.Id);
            response.UserId.Should().Be(user.Id);
            response.Games.Should().HaveCount(2);
        }

        [Fact]
        public async Task Given_ValidLoggedUser_When_GettingEmptyLibrary_Then_ShouldReturnLibraryWithNoGames()
        {
            // Arrange
            var user = UserBuilder.Build();
            var library = LibraryBuilder.Build(user.Id);
            
            var useCase = BuildUseCase(user, library);
            var request = GetMyLibraryInputBuilder.Build();

            // Act
            var response = await useCase.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.LibraryId.Should().Be(library.Id);
            response.UserId.Should().Be(user.Id);
            response.Games.Should().BeEmpty();
        }

        [Fact]
        public async Task Given_LoggedUser_When_LibraryNotFound_Then_ShouldThrowNotFoundException()
        {
            // Arrange
            var user = UserBuilder.Build();
            
            var loggedUserMock = new Mock<ILoggedUser>();
            loggedUserMock
                .Setup(x => x.GetLoggedUserAsync())
                .ReturnsAsync(user);

            var readOnlyLibraryRepoMock = new Mock<IReadOnlyLibraryRepository>();
            readOnlyLibraryRepoMock
                .Setup(x => x.GetByUserIdWithGamesAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Library?)null);

            var loggerMock = new Mock<ILogger<GetMyLibraryUseCase>>();

            var useCase = new GetMyLibraryUseCase(
                loggedUserMock.Object,
                readOnlyLibraryRepoMock.Object,
                loggerMock.Object);

            var request = GetMyLibraryInputBuilder.Build();

            // Act
            Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Given_ValidLoggedUserWithGames_When_GettingLibrary_Then_ShouldReturnCorrectGameDetails()
        {
            // Arrange
            var user = UserBuilder.Build();
            var library = LibraryBuilder.Build(user.Id);
            
            var gameName = "The Witcher 3";
            var gameDescription = "Epic RPG Adventure";
            var currentPrice = 100.00m;
            var purchasePrice = 80.00m;

            var game = Game.Create(
                Name.Create(gameName),
                gameDescription,
                Price.Create(currentPrice),
                GameCategory.RPG);

            library.AddGame(game.Id, Price.Create(purchasePrice));

            var useCase = BuildUseCase(user, library);
            var request = GetMyLibraryInputBuilder.Build();

            // Act
            var response = await useCase.Handle(request, CancellationToken.None);

            // Assert
            response.Games.Should().HaveCount(1);
            var gameDto = response.Games.First();
            gameDto.Name.Should().Be(gameName);
            gameDto.Description.Should().Be(gameDescription);
            gameDto.CurrentPrice.Should().Be(currentPrice);
            gameDto.PurchasePrice.Should().Be(purchasePrice);
            gameDto.Category.Should().Be(GameCategory.RPG.ToString());
        }
    }
}
