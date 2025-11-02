using MediatR;

namespace FCG.Application.UseCases.Admin.DepositToWallet
{
    public interface IDepositToWalletUseCase : IRequestHandler<DepositToWalletRequest, DepositToWalletResponse>
    {
    }
}
