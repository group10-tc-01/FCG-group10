using FCG.Application.UseCases.Games.GetAll;
using FCG.Domain.Enum;
using FCG.Domain.Models.Pagination;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Games
{
    [Binding]
    public class GetAllGamesStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private GetAllGamesInput? _getAllGamesInput;
        private PagedListResponse<GetAllGamesOutput>? _getAllGamesResponse;
        private Exception? _exception;

        [Given(@"que existem jogos cadastrados no sistema")]
        public void GivenThatThereAreGamesRegisteredInTheSystem()
        {

        }

        [When(@"o usuario solicita a listagem de jogos com paginacao")]
        public async Task WhenUserRequestsGameListWithPagination()
        {
            try
            {
                _getAllGamesInput = _fixtureManager.GetAllGames.GetAllGamesInput;
                _getAllGamesResponse = await _fixtureManager.GetAllGames.GetAllGamesUseCase.Handle(
                    _getAllGamesInput,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o usuario solicita a listagem de jogos filtrando por nome")]
        public async Task WhenUserRequestsGameListFilteringByName()
        {
            try
            {
                _getAllGamesInput = _fixtureManager.GetAllGames.GetAllGamesInputWithNameFilter;
                _getAllGamesResponse = await _fixtureManager.GetAllGames.GetAllGamesUseCase.Handle(
                    _getAllGamesInput,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o usuario solicita a listagem de jogos filtrando por categoria")]
        public async Task WhenUserRequestsGameListFilteringByCategory()
        {
            try
            {
                _getAllGamesInput = _fixtureManager.GetAllGames.GetAllGamesInputWithCategoryFilter;
                _getAllGamesResponse = await _fixtureManager.GetAllGames.GetAllGamesUseCase.Handle(
                    _getAllGamesInput,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o usuario solicita a listagem de jogos filtrando por faixa de preco")]
        public async Task WhenUserRequestsGameListFilteringByPriceRange()
        {
            try
            {
                _getAllGamesInput = _fixtureManager.GetAllGames.GetAllGamesInputWithPriceFilter;
                _getAllGamesResponse = await _fixtureManager.GetAllGames.GetAllGamesUseCase.Handle(
                    _getAllGamesInput,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o usuario solicita a listagem de jogos com multiplos filtros aplicados")]
        public async Task WhenUserRequestsGameListWithMultipleFiltersApplied()
        {
            try
            {
                _getAllGamesInput = _fixtureManager.GetAllGames.GetAllGamesInputWithMultipleFilters;
                _getAllGamesResponse = await _fixtureManager.GetAllGames.GetAllGamesUseCase.Handle(
                    _getAllGamesInput,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o sistema deve retornar a lista de jogos paginada com sucesso")]
        public void ThenTheSystemShouldReturnThePagedGameListSuccessfully()
        {
            _exception.Should().BeNull();
            _getAllGamesResponse.Should().NotBeNull();
            _getAllGamesResponse!.Items.Should().NotBeNull();
            _getAllGamesResponse.Items.Should().HaveCountGreaterThan(0);
            _getAllGamesResponse.TotalCount.Should().BeGreaterThan(0);
            _getAllGamesResponse.CurrentPage.Should().Be(_getAllGamesInput!.PageNumber);
            _getAllGamesResponse.PageSize.Should().Be(_getAllGamesInput.PageSize);
        }

        [Then(@"o sistema deve retornar apenas os jogos que correspondem ao filtro de nome")]
        public void ThenTheSystemShouldReturnOnlyGamesMatchingTheNameFilter()
        {
            _exception.Should().BeNull();
            _getAllGamesResponse.Should().NotBeNull();
            _getAllGamesResponse!.Items.Should().NotBeNull();
            _getAllGamesResponse.Items.Should().HaveCountGreaterThan(0);
            _getAllGamesResponse.Items.Should().Contain(g => g.Name.Contains("Witcher"));
        }

        [Then(@"o sistema deve retornar apenas os jogos da categoria especificada")]
        public void ThenTheSystemShouldReturnOnlyGamesFromTheSpecifiedCategory()
        {
            _exception.Should().BeNull();
            _getAllGamesResponse.Should().NotBeNull();
            _getAllGamesResponse!.Items.Should().NotBeNull();
            _getAllGamesResponse.Items.Should().HaveCountGreaterThan(0);
            _getAllGamesResponse.Items.Should().Contain(g => g.Category == GameCategory.RPG);
        }

        [Then(@"o sistema deve retornar apenas os jogos dentro da faixa de preco")]
        public void ThenTheSystemShouldReturnOnlyGamesWithinThePriceRange()
        {
            _exception.Should().BeNull();
            _getAllGamesResponse.Should().NotBeNull();
            _getAllGamesResponse!.Items.Should().NotBeNull();
            _getAllGamesResponse.Items.Should().HaveCountGreaterThan(0);
            _getAllGamesResponse.Items.Should().Contain(g =>
                g.Price >= _getAllGamesInput!.MinPrice &&
                g.Price <= _getAllGamesInput.MaxPrice);
        }

        [Then(@"o sistema deve retornar apenas os jogos que atendem a todos os filtros")]
        public void ThenTheSystemShouldReturnOnlyGamesThatMeetAllFilters()
        {
            _exception.Should().BeNull();
            _getAllGamesResponse.Should().NotBeNull();
            _getAllGamesResponse!.Items.Should().NotBeNull();
            _getAllGamesResponse.Items.Should().HaveCountGreaterThan(0);
            _getAllGamesResponse.Items.Should().Contain(g =>
                g.Name.Contains("Dark") &&
                g.Category == GameCategory.RPG &&
                g.Price >= 30m &&
                g.Price <= 50m);
        }
    }
}