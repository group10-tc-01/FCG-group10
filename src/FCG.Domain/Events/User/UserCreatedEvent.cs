namespace FCG.Domain.Events.User
{
    public class UserCreatedEvent : UserBaseEvent
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public UserCreatedEvent(Guid userId, string name, string email) : base(userId)
        {
            Name = name;
            Email = email;
        }
    }
}
