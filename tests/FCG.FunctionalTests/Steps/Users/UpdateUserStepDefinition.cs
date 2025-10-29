using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Users
{
    [Binding]
    public class UpdateUserStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private Exception? _exception;

        [Given(@"um usuario existente que deseja atualizar sua senha")]
        public void GivenAnExistingUserWhoWantsToUpdatePassword()
        {
        }

        [When(@"o usuario envia uma requisicao de atualizacao com nova senha valida")]
        public async Task WhenUserSendsUpdateRequestWithValidNewPassword()
        {
            try
            {
                await _fixtureManager.UpdateUser.UpdateUserUseCase.Handle(_fixtureManager.UpdateUser.UpdateUserRequest, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"a senha do usuario deve ser atualizada com sucesso")]
        public void ThenTheUserPasswordShouldBeUpdatedSuccessfully()
        {
            _exception.Should().BeNull();
        }
    }
}
