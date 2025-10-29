using FCG.Application.UseCases.Admin.GetAllUsers;
using FCG.Domain.Models.Pagination;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Admin
{
    [Binding]
    public class GetAllUsersStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private PagedListResponse<GetAllUsersResponse>? _getAllUsersResponse;
        private Exception? _exception;

        [Given(@"que existem usuarios cadastrados no sistema")]
        public void GivenThatThereAreUsersRegisteredInTheSystem()
        {
        }

        [When(@"o admin solicita a listagem de todos os usuarios")]
        public async Task WhenAdminRequestsListOfAllUsers()
        {
            try
            {
                _getAllUsersResponse = await _fixtureManager.GetAllUsers.GetAllUsersUseCase.Handle(
                    _fixtureManager.GetAllUsers.GetAllUserCaseRequest,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o sistema deve retornar a lista de usuarios com sucesso")]
        public void ThenTheSystemShouldReturnTheListOfUsersSuccessfully()
        {
            _exception.Should().BeNull();
            _getAllUsersResponse.Should().NotBeNull();
            _getAllUsersResponse!.Items.Should().NotBeNull();
            _getAllUsersResponse.Items.Should().HaveCountGreaterThan(0);
        }
    }
}
