using FCG.Application.UseCases.Authentication.Logout;
using FCG.FunctionalTests.Fixtures;
using FCG.Messages;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Authentication
{
    [Binding]
    public class LogoutStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private LogoutInput? _logoutInput;
        private LogoutOutput? _logoutOutput;
        private Exception? _exception;


        [Given(@"que o usuario possui um token valido")]
        public void GivenThatUserHasValidToken()
        {
            _logoutInput = _fixtureManager.Logout.LogoutInputWithUserId;
        }


        [When(@"o usuario envia uma requisicao de logout com userid valido")]
        public async Task WhenUserSendsRequestToLogoutWithValidUserId()
        {
            try
            {
                _logoutOutput = await _fixtureManager.Logout.LogoutUseCase.Handle(_logoutInput!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"todos os refresh tokens do usuario devem ser revogados")]
        public void ThenAllUserRefreshTokensShouldBeRevoked()
        {
            _logoutOutput.Should().NotBeNull();
            _logoutOutput.Success.Should().BeTrue();
        }

        [Then(@"o sistema deve retornar mensagem de sucesso")]
        public void ThenTheSystemShouldReturnSuccessMessage()
        {
            _logoutOutput.Should().NotBeNull();
            _logoutOutput.Success.Should().BeTrue();
            _logoutOutput.Message.Should().Be(ResourceMessages.LogoutSuccessFull);
        }
    }
}