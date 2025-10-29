using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;

namespace FCG.Domain.Entities
{
    public sealed class Promotion : BaseEntity
    {
        public Guid GameId { get; private set; }
        public Discount Discount { get; private set; } = null!;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Game? Game { get; }

        private Promotion(Guid gameId, Discount discount, DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new DomainException(ResourceMessages.PromotionEndDateMustBeAfterStartDate);
            }

            GameId = gameId;
            Discount = discount;
            StartDate = startDate;
            EndDate = endDate;
        }

        private Promotion() { }

        public static Promotion Create(Guid gameId, Discount discount, DateTime startDate, DateTime endDate)
        {
            return new Promotion(gameId, discount, startDate, endDate);
        }
    }
}
