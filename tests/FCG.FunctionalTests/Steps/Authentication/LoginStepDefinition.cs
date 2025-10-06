using FCG.Application.UseCases.Authentication.Login;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Authentication
{

    [Binding]
    public class LoginStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private LoginInput? _loginInput;
        private LoginOutput? _loginOutput;
        private Exception? _exception;

        [Given(@"que o usuario deseja realizar o login")]
        public void GivenThatUserWantsToLogin()
        {
            _loginInput = _fixtureManager.Login.LoginInput;
        }

        [When(@"o usuario envia uma requisicao de login")]
        public async Task WhenUserSendsRequestToLogin()
        {
            try
            {
                _loginOutput = await _fixtureManager.Login.LoginUseCase.Handle(_loginInput!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o sistema deve autenticar o usuario com sucesso")]
        public void ThenTheUserShouldBeAuthenticatedSuccessfully()
        {
            _exception.Should().BeNull();
            _loginOutput.Should().NotBeNull();
            _loginOutput.AccessToken.Should().NotBeNullOrEmpty();
            _loginOutput.RefreshToken.Should().NotBeNullOrEmpty();
            _loginOutput.ExpiresInMinutes.Should().BeGreaterThan(0);
        }
    }
}
