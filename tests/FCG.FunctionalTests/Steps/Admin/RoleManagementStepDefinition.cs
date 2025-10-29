using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Admin
{
    [Binding]
    public class RoleManagementStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private Exception? _exception;

        [Given(@"um usuario que precisa ter seu perfil de admin alterado")]
        public void GivenAUserWhoNeedsToHaveTheirAdminProfileChanged()
        {
        }

        [When(@"o admin altera o perfil de administrador do usuario")]
        public async Task WhenAdminChangesTheUserAdministratorProfile()
        {
            try
            {
                await _fixtureManager.RoleManagement.RoleManagementUseCase.Handle(
                    _fixtureManager.RoleManagement.RoleManagementRequest,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o perfil do usuario deve ser alterado com sucesso")]
        public void ThenTheUserProfileShouldBeChangedSuccessfully()
        {
            _exception.Should().BeNull();
        }
    }
}
