using FCG.Application.UseCases.Admin.GetById;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Admin
{
    [Binding]
    public class GetUserByIdStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private GetUserByIdResponse? _getUserByIdResponse;
        private Exception? _exception;

        [Given(@"um usuario existente no sistema")]
        public void GivenAnExistingUserInTheSystem()
        {
        }

        [When(@"o admin solicita os detalhes do usuario por ID")]
        public async Task WhenAdminRequestsUserDetailsByID()
        {
            try
            {
                _getUserByIdResponse = await _fixtureManager.GetUserById.GetUserByIdUseCase.Handle(
                    _fixtureManager.GetUserById.GetUserByIdRequest,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o sistema deve retornar os detalhes do usuario com sucesso")]
        public void ThenTheSystemShouldReturnUserDetailsSuccessfully()
        {
            _exception.Should().BeNull();
            _getUserByIdResponse.Should().NotBeNull();
            _getUserByIdResponse!.Id.Should().NotBeEmpty();
            _getUserByIdResponse.Id.Should().Be(_fixtureManager.GetUserById.GetUserByIdRequest.Id);
            _getUserByIdResponse.Name.Should().NotBeNullOrEmpty();
            _getUserByIdResponse.Email.Should().NotBeNullOrEmpty();
        }
    }
}
