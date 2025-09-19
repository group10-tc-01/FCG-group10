using System.Dynamic;
using System.Runtime.CompilerServices;

namespace FCG.Domain.Entities
{
    public class Promotion : BaseEntity
    {
        public Guid GameId { get; private set; }
        public decimal Discount { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public Game Game { get; set; }

        protected Promotion()
        {
        }
        public Promotion(Guid gameId, decimal discount, DateTime startDate, DateTime endDate)
        {
            if (discount <= 0)
            {
                throw new ArgumentException("Disconto deve ser maior que zero.");
            }
            GameId = gameId;
            Discount = discount;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }

    }
}
