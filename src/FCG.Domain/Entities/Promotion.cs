using System.Dynamic;
using System.Runtime.CompilerServices;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class Promotion : BaseEntity
    {
        public Guid GameId { get; private set; }
        public Discount Discount { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }


        public Game Game { get; set; }

        public Promotion()
        {
        }
        private Promotion(Guid gameId, decimal discount, DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("End date must be on or after the start date.");
            }

            GameId = gameId;
            Discount = discount;
            StartDate = startDate;
            EndDate = endDate;

        }
        public static Promotion Create(Guid gameId, decimal discount, DateTime startDate, DateTime endDate)
        {
            return new Promotion(gameId, discount, startDate, endDate);
        }

    }
}
