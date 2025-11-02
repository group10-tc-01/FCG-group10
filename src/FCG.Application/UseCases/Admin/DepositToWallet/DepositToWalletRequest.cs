using MediatR;

namespace FCG.Application.UseCases.Admin.DepositToWallet
{
    public class DepositToWalletRequest : IRequest<DepositToWalletResponse>
    {
        public Guid UserId { get; init; }
        public Guid WalletId { get; init; }
        public decimal Amount { get; init; }
    }
}
