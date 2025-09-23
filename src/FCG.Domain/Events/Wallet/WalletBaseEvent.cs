using MediatR;

namespace FCG.Domain.Events.Wallet
{
    public class WalletBaseEvent : INotification
    {
        public Guid WalletId { get; set; }
        public DateTime OcurredOn { get; set; }

        public WalletBaseEvent(Guid walletId)
        {
            WalletId = walletId;
            OcurredOn = DateTime.Now;
        }
    }
}
