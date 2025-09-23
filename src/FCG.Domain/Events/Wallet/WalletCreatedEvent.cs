namespace FCG.Domain.Events.Wallet
{
    public class WalletCreatedEvent : WalletBaseEvent
    {
        public Guid UserId { get; set; }
        public decimal InitialBalance { get; set; }

        public WalletCreatedEvent(Guid walletId, Guid userId, decimal initialBalance)
            : base(walletId)
        {
            UserId = userId;
            InitialBalance = initialBalance;
        }
    }
}
