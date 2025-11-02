using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.FunctionalTests.Fixtures.Library;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Library
{
    [Binding]
    public class GetMyLibraryStepDefinition
    {
        private readonly GetMyLibraryFixture _fixture;
        private FCG.Application.UseCases.Library.GetMyLibrary.GetMyLibraryResponse? _response;
        private Exception? _exception;

        public GetMyLibraryStepDefinition()
        {
            _fixture = new GetMyLibraryFixture();
        }

        [Given(@"I am an authenticated user")]
        public void GivenIAmAnAuthenticatedUser()
        {
            _fixture.LoggedUser.Should().NotBeNull();
        }

        [Given(@"I am an authenticated admin user")]
        public void GivenIAmAnAuthenticatedAdminUser()
        {
            _fixture.LoggedUser.Should().NotBeNull();
        }

        [Given(@"I am not authenticated")]
        public void GivenIAmNotAuthenticated()
        {
            // Simulated by not having a valid user in the context
        }

        [Given(@"I have games in my library")]
        public void GivenIHaveGamesInMyLibrary()
        {
            _fixture.AddGameToLibrary(
                "The Witcher 3",
                "Epic RPG Adventure",
                100.00m,
                80.00m,
                GameCategory.RPG);

            _fixture.AddGameToLibrary(
                "Cyberpunk 2077",
                "Futuristic open world",
                120.00m,
                100.00m,
                GameCategory.Action);
        }

        [Given(@"I have no games in my library")]
        public void GivenIHaveNoGamesInMyLibrary()
        {
            // Library is already empty by default
            _fixture.Library.LibraryGames.Should().BeEmpty();
        }

        [When(@"I request to get my library")]
        public async Task WhenIRequestToGetMyLibrary()
        {
            try
            {
                _response = await _fixture.GetMyLibraryUseCase.Handle(
                    _fixture.GetMyLibraryRequest,
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int expectedStatusCode)
        {
            if (expectedStatusCode == 200)
            {
                _response.Should().NotBeNull();
                _exception.Should().BeNull();
            }
            else if (expectedStatusCode == 401)
            {
                // In a real scenario, this would be handled by authentication middleware
                // For functional tests, we're testing the use case directly
                _exception.Should().NotBeNull();
            }
        }

        [Then(@"the library should contain my games")]
        public void ThenTheLibraryShouldContainMyGames()
        {
            _response.Should().NotBeNull();
            _response!.Games.Should().NotBeEmpty();
            _response.Games.Should().HaveCountGreaterThan(0);
        }

        [Then(@"the library should be empty")]
        public void ThenTheLibraryShouldBeEmpty()
        {
            _response.Should().NotBeNull();
            _response!.Games.Should().BeEmpty();
        }
    }
}
