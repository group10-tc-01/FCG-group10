namespace FCG.Domain.Events.Wallet
{
    public record WalletCreatedEvent(Guid WalletId, Guid UserId) : DomainEvent;
}
