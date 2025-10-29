using FCG.Application.UseCases.Games.GetAll;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories.GameRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.CommomTestsUtilities.Extensions;
using FCG.Domain.Entities;
using FCG.Domain.Enum;

namespace FCG.FunctionalTests.Fixtures.Games
{
    public class GetAllGamesFixture
    {
        public GetAllGamesFixture()
        {
            var readOnlyGameRepository = ReadOnlyGameRepositoryBuilder.Build();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<GetAllGamesUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();

            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            Setup();

            GetAllGamesUseCase = new GetAllGamesUseCase(readOnlyGameRepository, logger, correlationIdProvider);

            GetAllGamesInput = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10
            };

            GetAllGamesInputWithNameFilter = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Name = "Witcher"
            };

            GetAllGamesInputWithCategoryFilter = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Category = GameCategory.RPG
            };

            GetAllGamesInputWithPriceFilter = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                MinPrice = 20m,
                MaxPrice = 50m
            };

            GetAllGamesInputWithMultipleFilters = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Name = "Dark",
                Category = GameCategory.RPG,
                MinPrice = 30m,
                MaxPrice = 50m
            };
        }

        public GetAllGamesUseCase GetAllGamesUseCase { get; }
        public GetAllGamesInput GetAllGamesInput { get; }
        public GetAllGamesInput GetAllGamesInputWithNameFilter { get; }
        public GetAllGamesInput GetAllGamesInputWithCategoryFilter { get; }
        public GetAllGamesInput GetAllGamesInputWithPriceFilter { get; }
        public GetAllGamesInput GetAllGamesInputWithMultipleFilters { get; }

        private static void Setup()
        {
            var games = new List<Game>
            {
                GameBuilder.BuildWithAllParameters("The Witcher 3", "RPG Game", 59.99m, GameCategory.RPG),
                GameBuilder.BuildWithAllParameters("Witcher 2", "RPG Game", 29.99m, GameCategory.RPG),
                GameBuilder.BuildWithAllParameters("Dark Souls", "RPG Game", 39.99m, GameCategory.RPG),
                GameBuilder.BuildWithAllParameters("Dark Souls 3", "RPG Game", 49.99m, GameCategory.RPG),
                GameBuilder.BuildWithAllParameters("Cyberpunk 2077", "Action Game", 59.99m, GameCategory.Action),
                GameBuilder.BuildWithAllParameters("God of War", "Action Game", 49.99m, GameCategory.Action),
                GameBuilder.BuildWithAllParameters("Budget Game", "Indie Game", 9.99m, GameCategory.Puzzle),
                GameBuilder.BuildWithAllParameters("Premium Game", "AAA Game", 69.99m, GameCategory.Adventure)
            }.AsQueryable();

            var queryable = games.AsQueryable().BuildMockDbSet();
            ReadOnlyGameRepositoryBuilder.SetupGetAllAsQueryable(queryable);
        }
    }
}