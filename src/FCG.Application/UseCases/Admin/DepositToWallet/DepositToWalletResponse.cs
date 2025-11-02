namespace FCG.Application.UseCases.Admin.DepositToWallet
{
    public class DepositToWalletResponse
    {
        public Guid UserId { get; init; }
        public decimal NewBalance { get; init; }
        public decimal DepositedAmount { get; init; }
    }
}
