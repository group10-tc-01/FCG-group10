namespace FCG.Domain.Entities
{
    public sealed class Library : BaseEntity
    {
        public Guid UserId { get; private set; }

        public User User { get; set; }
        public ICollection<LibraryGame> LibraryGames { get; set; }

        public Library(Guid userId)
        {
            UserId = userId;
            LibraryGames = new List<LibraryGame>();

        }

        public static Library Create(Guid userId)
        {
            return new Library(userId);
        }
    }
}
