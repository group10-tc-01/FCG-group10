using MediatR;

namespace FCG.Domain.Events.Promotion
{
    public class PromotionBaseEvent : INotification
    {
        public Guid PromotionId { get; set; }
        public DateTime OcurredOn { get; set; }

        public PromotionBaseEvent(Guid promotionId)
        {
            PromotionId = promotionId;
            OcurredOn = DateTime.Now;
        }
    }
}
