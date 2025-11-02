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

        [Given(@"um administrador que deseja depositar valor na carteira de um usuario")]
        public void GivenAnAdministratorWhoWantsToDepositAmountToUserWallet()
        {
            _initialBalance = _fixtureManager.DepositToWallet.GetInitialBalance();
            _fixtureManager.DepositToWallet.SetupForExistingWallet();
        }

        [Given(@"um administrador que tenta depositar em uma carteira que nao existe")]
        public void GivenAnAdministratorWhoTriesToDepositToNonExistentWallet()
        {
            _fixtureManager.DepositToWallet.SetupForNonExistentWallet();
        }

        [When(@"o administrador realiza o deposito")]
        public async Task WhenTheAdministratorPerformsTheDeposit()
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

        [Then(@"o saldo da carteira deve ser atualizado com sucesso")]
        public void ThenTheWalletBalanceShouldBeUpdatedSuccessfully()
        {
            _exception.Should().BeNull();
            _response.Should().NotBeNull();
            _response!.DepositedAmount.Should().Be(100.00m);
            _response.NewBalance.Should().Be(_initialBalance + 100.00m);
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
