namespace FCG.Domain.Entities
{
    public class Library : BaseEntity
    {
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public bool IsActive { get; private set; } = true;


        public User User { get; set; }
        public ICollection<LibraryGame> LibraryGames { get; set; }

        protected Library()
        {
        }
        public Library(Guid userId)
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }

    }
}
