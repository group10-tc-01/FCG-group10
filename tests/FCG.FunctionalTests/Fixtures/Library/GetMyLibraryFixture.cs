using FCG.Application.UseCases.Library.GetMyLibrary;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories.LibraryRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;

namespace FCG.FunctionalTests.Fixtures.Library
{
    public class GetMyLibraryFixture
    {
        public User LoggedUser { get; }
        public Domain.Entities.Library Library { get; }
        public GetMyLibraryUseCase GetMyLibraryUseCase { get; }
        public GetMyLibraryRequest GetMyLibraryRequest { get; }

        public GetMyLibraryFixture()
        {
            LoggedUser = UserBuilder.Build();
            Library = LibraryBuilder.Build(LoggedUser.Id);
            
            var loggedUserService = LoggedUserServiceBuilder.Build();
            var readOnlyLibraryRepository = ReadOnlyLibraryRepositoryBuilder.Build();
            var logger = new NullLogger<GetMyLibraryUseCase>();

            Setup();

            GetMyLibraryUseCase = new GetMyLibraryUseCase(
                loggedUserService,
                readOnlyLibraryRepository,
                logger
            );

            GetMyLibraryRequest = new GetMyLibraryRequest();
        }

        public void AddGameToLibrary(string gameName, string description, decimal currentPrice, decimal purchasePrice, GameCategory category)
        {
            var game = Game.Create(
                Name.Create(gameName),
                description,
                Price.Create(currentPrice),
                category);

            Library.AddGame(game.Id, Price.Create(purchasePrice));

            Setup();
        }

        private void Setup()
        {
            LoggedUserServiceBuilder.SetupGetLoggedUserAsync(LoggedUser);
            ReadOnlyLibraryRepositoryBuilder.SetupGetByUserIdWithGamesAsync(Library);
        }
    }
}
