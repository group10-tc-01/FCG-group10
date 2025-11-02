using FCG.Application.UseCases.Admin.DepositToWallet;
using FCG.Domain.Exceptions;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Admin
{
    [Binding]
    public class DepositToWalletStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private DepositToWalletResponse? _response;
        private Exception? _exception;
        private decimal _initialBalance;

        [Given(@"um administrador que tenta depositar em uma carteira que nao existe")]
        public void GivenAnAdministratorWhoTriesToDepositToNonExistentWallet()
        {
            _fixtureManager.DepositToWallet.SetupForNonExistentWallet();
        }

        [When(@"o administrador tenta realizar o deposito")]
        public async Task WhenTheAdministratorTriesToPerformTheDeposit()
        {
            try
            {
                _response = await _fixtureManager.DepositToWallet.DepositToWalletUseCase.Handle(
                    _fixtureManager.DepositToWallet.DepositRequest,
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"deve ocorrer um erro de carteira nao encontrada")]
        public void ThenAWalletNotFoundErrorShouldOccur()
        {
            _exception.Should().NotBeNull();
            _exception.Should().BeOfType<NotFoundException>();
            _response.Should().BeNull();
        }
    }
}
