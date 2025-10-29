using FCG.Application.UseCases.Games.GetAll;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories.GameRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.CommomTestsUtilities.Extensions;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.GamesRepository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Games.GetAllGames
{
    public class GetAllGamesUseCaseTest
    {
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;
        private readonly IGetAllGamesUseCase _sut;

        public GetAllGamesUseCaseTest()
        {
            _readOnlyGameRepository = ReadOnlyGameRepositoryBuilder.Build();
            var logger = new Mock<ILogger<GetAllGamesUseCase>>().Object;
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            _sut = new GetAllGamesUseCase(_readOnlyGameRepository, logger, correlationIdProvider);
        }

        [Fact(DisplayName = "Deve retornar lista paginada de jogos quando não há filtros")]
        public async Task Given_ValidRequestWithoutFilters_When_Handle_Then_ShouldReturnPagedGames()
        {
            // Arrange
            var games = GameBuilder.BuildList(10);
            SetupGetAllAsQueryable(games);

            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 5,
                Filter = new GameFilter()
            };

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(5);
            result.TotalCount.Should().Be(10);
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(5);

            ReadOnlyGameRepositoryBuilder.VerifyGetAllAsQueryableWasCalled();
        }

        [Fact(DisplayName = "Deve aplicar filtro por nome corretamente")]
        public async Task Given_FilterWithName_When_Handle_Then_ShouldReturnFilteredGames()
        {
            // Arrange
            var games = new List<Game>
            {
                GameBuilder.BuildWithName("FIFA 24"),
                GameBuilder.BuildWithName("NBA 2K"),
                GameBuilder.BuildWithName("FIFA Street")
            };

            SetupGetAllAsQueryable(games);

            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter { Name = "FIFA" }
            };

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Name.Contains("FIFA")).Should().BeTrue();
        }

        [Fact(DisplayName = "Deve aplicar filtro por categoria corretamente")]
        public async Task Given_FilterWithCategory_When_Handle_Then_ShouldReturnFilteredGames()
        {
            // Arrange
            var games = new List<Game>
            {
                GameBuilder.BuildWithCategory(GameCategory.Action),
                GameBuilder.BuildWithCategory(GameCategory.Sports),
                GameBuilder.BuildWithCategory(GameCategory.Action)
            };

            SetupGetAllAsQueryable(games);

            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter { Category = GameCategory.Action }
            };

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Category == GameCategory.Action).Should().BeTrue();
        }

        [Fact(DisplayName = "Deve aplicar filtro por preço mínimo corretamente")]
        public async Task Given_FilterWithMinPrice_When_Handle_Then_ShouldReturnFilteredGames()
        {
            // Arrange
            var games = new List<Game>
            {
                GameBuilder.BuildWithPrice(20),
                GameBuilder.BuildWithPrice(50),
                GameBuilder.BuildWithPrice(100)
            };

            SetupGetAllAsQueryable(games);

            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter { MinPrice = 50 }
            };

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Price >= 50).Should().BeTrue();
        }

        [Fact(DisplayName = "Deve aplicar filtro por preço máximo corretamente")]
        public async Task Given_FilterWithMaxPrice_When_Handle_Then_ShouldReturnFilteredGames()
        {
            // Arrange
            var games = new List<Game>
            {
                GameBuilder.BuildWithPrice(20),
                GameBuilder.BuildWithPrice(50),
                GameBuilder.BuildWithPrice(100)
            };

            SetupGetAllAsQueryable(games);

            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter { MaxPrice = 50 }
            };

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Price <= 50).Should().BeTrue();
        }

        [Fact(DisplayName = "Deve aplicar filtro por faixa de preço corretamente")]
        public async Task Given_FilterWithMinAndMaxPrice_When_Handle_Then_ShouldReturnFilteredGames()
        {
            // Arrange
            var games = new List<Game>
            {
                GameBuilder.BuildWithPrice(10),
                GameBuilder.BuildWithPrice(50),
                GameBuilder.BuildWithPrice(75),
                GameBuilder.BuildWithPrice(120)
            };

            SetupGetAllAsQueryable(games);

            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter { MinPrice = 40, MaxPrice = 100 }
            };

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Price >= 40 && x.Price <= 100).Should().BeTrue();
        }

        [Fact(DisplayName = "Deve aplicar paginação corretamente")]
        public async Task Given_PaginationParams_When_Handle_Then_ShouldReturnCorrectPage()
        {
            // Arrange
            var games = GameBuilder.BuildList(12);
            SetupGetAllAsQueryable(games);

            var input = new GetAllGamesInput
            {
                PageNumber = 2,
                PageSize = 5,
                Filter = new GameFilter()
            };

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(5);
            result.TotalCount.Should().Be(12);
            result.CurrentPage.Should().Be(2);
            result.PageSize.Should().Be(5);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando nenhum jogo corresponde aos filtros")]
        public async Task Given_FilterWithNoMatches_When_Handle_Then_ShouldReturnEmptyList()
        {
            // Arrange
            var games = new List<Game>
            {
                GameBuilder.BuildWithName("FIFA 24"),
                GameBuilder.BuildWithName("NBA 2K")
            };

            SetupGetAllAsQueryable(games);

            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter { Name = "Call of Duty" }
            };

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        private static void SetupGetAllAsQueryable(IEnumerable<Game> games)
        {
            var queryable = games.AsQueryable().BuildMockDbSet();
            ReadOnlyGameRepositoryBuilder.SetupGetAllAsQueryable(queryable);
        }
    }
}
