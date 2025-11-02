using FCG.Application.UseCases.Admin.CreateUser;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Admin
{
    [Binding]
    public class CreateUserByAdminStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private CreateUserByAdminResponse? _response;
        private Exception? _exception;

        [Given(@"um administrador que deseja criar um novo usuario com perfil comum")]
        public void GivenAnAdministratorWhoWantsToCreateANewUserWithCommonProfile()
        {
            _fixtureManager.CreateUserByAdmin.SetupForNewUser();
        }

        [Given(@"um administrador que deseja criar um novo usuario com perfil de administrador")]
        public void GivenAnAdministratorWhoWantsToCreateANewUserWithAdministratorProfile()
        {
            _fixtureManager.CreateUserByAdmin.SetupForNewUser();
        }

        [Given(@"um administrador que tenta criar um usuario com email ja existente")]
        public void GivenAnAdministratorWhoTriesToCreateAUserWithExistingEmail()
        {
            _fixtureManager.CreateUserByAdmin.SetupForDuplicateEmail();
        }

        [When(@"o administrador cria o usuario")]
        public async Task WhenTheAdministratorCreatesTheUser()
        {
            try
            {
                _response = await _fixtureManager.CreateUserByAdmin.CreateUserByAdminUseCase.Handle(
                    _fixtureManager.CreateUserByAdmin.CreateUserRequest,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o administrador cria o usuario administrador")]
        public async Task WhenTheAdministratorCreatesTheAdminUser()
        {
            try
            {
                _response = await _fixtureManager.CreateUserByAdmin.CreateUserByAdminUseCase.Handle(
                    _fixtureManager.CreateUserByAdmin.CreateAdminRequest,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"o administrador tenta criar o usuario")]
        public async Task WhenTheAdministratorTriesToCreateTheUser()
        {
            try
            {
                _response = await _fixtureManager.CreateUserByAdmin.CreateUserByAdminUseCase.Handle(
                    _fixtureManager.CreateUserByAdmin.CreateUserRequest,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o usuario deve ser criado com sucesso com perfil de usuario comum")]
        public void ThenTheUserShouldBeCreatedSuccessfullyWithCommonUserProfile()
        {
            _exception.Should().BeNull();
            _response.Should().NotBeNull();
            _response!.Name.Should().Be("Test User");
            _response.Email.Should().Be("testuser@example.com");
            _response.Role.Should().Be(Role.User);
            _response.Id.Should().NotBeEmpty();
        }

        [Then(@"o usuario deve ser criado com sucesso com perfil de administrador")]
        public void ThenTheUserShouldBeCreatedSuccessfullyWithAdministratorProfile()
        {
            _exception.Should().BeNull();
            _response.Should().NotBeNull();
            _response!.Name.Should().Be("Test Admin");
            _response.Email.Should().Be("testadmin@example.com");
            _response.Role.Should().Be(Role.Admin);
            _response.Id.Should().NotBeEmpty();
        }

        [Then(@"deve ocorrer um erro de email duplicado")]
        public void ThenADuplicateEmailErrorShouldOccur()
        {
            _exception.Should().NotBeNull();
            _exception.Should().BeOfType<DuplicateEmailException>();
            _response.Should().BeNull();
        }
    }
}
