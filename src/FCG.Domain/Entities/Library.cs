namespace FCG.Domain.Entities
{
    public sealed class Library : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User? User { get; private set; }
        public ICollection<LibraryGame>? LibraryGames { get; private set; }

        public Library(Guid userId)
        {
            UserId = userId;
        }

        public static Library Create(Guid userId)
        {
            return new Library(userId);
        }
    }
}
