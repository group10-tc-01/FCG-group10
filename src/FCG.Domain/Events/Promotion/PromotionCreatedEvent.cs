namespace FCG.Domain.Events.Promotion
{
    public record PromotionCreatedEvent(
        Guid PromotionId,
        Guid GameId
    ) : DomainEvent;
}
