namespace FCG.Domain.Events.User
{
    public class UserBaseEvent : IDomainEvent
    {
        public Guid UserId { get; set; }
        public DateTime OcurredOn { get; set; }

        public UserBaseEvent(Guid userId)
        {
            UserId = userId;
            OcurredOn = DateTime.Now;
        }
    }
}
