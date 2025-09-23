namespace FCG.Domain.Events.Promotion
{
    public class PromotionCreatedEvent : PromotionBaseEvent
    {
        public Guid GameId { get; set; }
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public PromotionCreatedEvent(Guid promotionId, Guid gameId, decimal discount, DateTime startDate, DateTime endDate)
            : base(promotionId)
        {
            GameId = gameId;
            Discount = discount;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
