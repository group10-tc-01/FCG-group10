using FCG.Application.UseCases.Authentication.RefreshToken;
using FCG.Domain.Exceptions;
using FCG.FunctionalTests.Fixtures;
using FCG.Messages;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Authentication
{
    [Binding]
    public class RefreshTokenStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private RefreshTokenInput? _refreshTokenInput;
        private RefreshTokenOutput? _refreshTokenOutput;
        private Exception? _exception;

        [Given(@"que o usuario deseja gerar um novo token de acesso")]
        public void GivenThatUserWantsToGenerateNewAccessToken()
        {
            _refreshTokenInput = _fixtureManager.RefreshToken.RefreshTokenInput;
        }

        [Given(@"que o usuario possui um refresh token valido")]
        public void GivenThatUserHasValidRefreshToken()
        {
            _refreshTokenInput = _fixtureManager.RefreshToken.RefreshTokenInputWithValidToken;
        }

        [Given(@"que o usuario possui um refresh token invalido")]
        public void GivenThatUserHasInvalidRefreshToken()
        {
            _refreshTokenInput = _fixtureManager.RefreshToken.RefreshTokenInputWithInvalidToken;
        }

        [Given(@"que o usuario possui um refresh token expirado")]
        public void GivenThatUserHasExpiredRefreshToken()
        {
            _refreshTokenInput = _fixtureManager.RefreshToken.RefreshTokenInputWithInvalidToken;
        }

        [When(@"o usuario envia uma requisicao de refresh token")]
        public async Task WhenUserSendsRefreshTokenRequest()
        {
            try
            {
                _refreshTokenOutput = await _fixtureManager.RefreshToken.RefreshTokenUseCase.Handle(_refreshTokenInput!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o usuario envia uma requisicao de refresh token com token valido")]
        public async Task WhenUserSendsRefreshTokenRequestWithValidToken()
        {
            try
            {
                _refreshTokenOutput = await _fixtureManager.RefreshToken.RefreshTokenUseCase.Handle(_refreshTokenInput!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o usuario envia uma requisicao de refresh token com token invalido")]
        public async Task WhenUserSendsRefreshTokenRequestWithInvalidToken()
        {
            try
            {
                _refreshTokenOutput = await _fixtureManager.RefreshToken.RefreshTokenUseCase.Handle(_refreshTokenInput!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o usuario envia uma requisicao de refresh token com token expirado")]
        public async Task WhenUserSendsRefreshTokenRequestWithExpiredToken()
        {
            try
            {
                _refreshTokenOutput = await _fixtureManager.RefreshToken.RefreshTokenUseCase.Handle(_refreshTokenInput!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o usuario deve receber um novo token de acesso com sucesso")]
        public void ThenTheUserShouldReceiveNewAccessTokenSuccessfully()
        {
            _exception.Should().BeNull();
            _refreshTokenOutput.Should().NotBeNull();
            _refreshTokenOutput.AccessToken.Should().NotBeNullOrEmpty();
            _refreshTokenOutput.RefreshToken.Should().NotBeNullOrEmpty();
            _refreshTokenOutput.ExpiresInDays.Should().BeGreaterThan(0);
        }

        [Then(@"o sistema deve gerar um novo access token")]
        public void ThenTheSystemShouldGenerateNewAccessToken()
        {
            _refreshTokenOutput.Should().NotBeNull();
            _refreshTokenOutput.AccessToken.Should().NotBeNullOrEmpty();
            _refreshTokenOutput.AccessToken.Should().Be("new_access_token");
        }

        [Then(@"o sistema deve gerar um novo refresh token")]
        public void ThenTheSystemShouldGenerateNewRefreshToken()
        {
            _refreshTokenOutput.Should().NotBeNull();
            _refreshTokenOutput.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Then(@"o sistema deve retornar o tempo de expiracao")]
        public void ThenTheSystemShouldReturnExpirationTime()
        {
            _refreshTokenOutput.Should().NotBeNull();
            _refreshTokenOutput.ExpiresInDays.Should().BeGreaterThan(0);
        }

        [Then(@"o sistema deve retornar erro de token invalido")]
        public void ThenTheSystemShouldReturnInvalidTokenError()
        {
            _exception.Should().NotBeNull();
            _exception.Should().BeOfType<UnauthorizedException>();
            _exception.Message.Should().Be(ResourceMessages.InvalidRefreshToken);
        }

        [Then(@"o sistema deve retornar erro de token expirado")]
        public void ThenTheSystemShouldReturnExpiredTokenError()
        {
            _exception.Should().NotBeNull();
            _exception.Should().BeOfType<UnauthorizedException>();
            _exception.Message.Should().Be(ResourceMessages.InvalidRefreshToken);
        }
    }
}