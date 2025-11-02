using FCG.Application.UseCases.Games.Register;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Games
{
    [Binding]
    public class RegisterGameStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private RegisterGameInput? _registerGameInput;
        private RegisterGameOutput? _registerGameOutput;
        private Exception? _exception;

        [Given(@"a criacao de um novo jogo")]
        public void GivenTheCreationOfANewGame()
        {
            _registerGameInput = _fixtureManager.RegisterGame.RegisterGameInput;
        }

        [When(@"o admin envia uma requisicao de criacao de jogo com dados validos")]
        public async Task WhenAdminSendsGameCreationRequestWithValidData()
        {
            try
            {
                _registerGameOutput = await _fixtureManager.RegisterGame.RegisterGameUseCase.Handle(_registerGameInput!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o jogo deve ser criado com sucesso")]
        public void ThenTheSystemShouldReturnSuccessMessage()
        {
            _exception.Should().BeNull();
            _registerGameOutput.Should().NotBeNull();
            _registerGameOutput.Id.Should().NotBeEmpty();
            _registerGameOutput.Name.Should().NotBeNullOrEmpty();
            _registerGameOutput.Name.Should().Be(_registerGameInput!.Name);
        }
    }
}