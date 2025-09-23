using MediatR;

namespace FCG.Domain.Events.Game
{
    public class GameBaseEvent : INotification
    {
        public Guid GameId { get; set; }
        public DateTime OcurredOn { get; set; }

        public GameBaseEvent(Guid gameId)
        {
            GameId = gameId;
            OcurredOn = DateTime.Now;
        }
    }
}
