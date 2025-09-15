using FCG.Application.UseCases.Example.CreateExample;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Example
{
    [Binding]
    public class CreateExampleStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private CreateExampleInput? _createExampleInput;
        private CreateExampleOutput? _createExampleOutput;
        private Exception? _exception;

        [Given(@"que o usuario deseja criar um exemplo")]
        public void GivenThatUserWantsToCreateAnExample()
        {
            _createExampleInput = _fixtureManager.CreateExample.CreateExampleInput;
        }

        [When(@"o usuario envia uma requisicao para criar um exemplo")]
        public async Task WhenUserSendsRequestToCreateExample()
        {
            try
            {
                _createExampleOutput = await _fixtureManager.CreateExample.CreateExampleUseCase.Handle(_createExampleInput!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o sistema deve criar o exemplo com sucesso")]
        public void ThenTheExampleShouldBeCreatedSuccessfully()
        {
            _exception.Should().BeNull();
            _createExampleOutput.Should().NotBeNull();
            _createExampleOutput.Example.Name.Should().Be(_createExampleInput!.Name);
            _createExampleOutput.Example.Description.Should().Be(_createExampleInput.Description);
        }
    }
}
