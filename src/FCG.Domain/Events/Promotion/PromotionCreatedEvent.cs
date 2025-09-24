namespace FCG.Domain.Events.Promotion
{
    public record PromotionCreatedEvent(
        Guid PromotionId,
        Guid GameId,
        decimal Discount,
        DateTime StartDate,
        DateTime EndDate
    ) : DomainEvent;
}
