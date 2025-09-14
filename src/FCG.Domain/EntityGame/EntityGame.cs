using FCG.Domain.EntityBase;

namespace FCG.Domain.EntityGame
{
    public class EntityGame : Base
    {
        public string Title { get; private set; } = string.Empty;
        public string Genre { get; private set; } = string.Empty;
        public DateTime ReleaseDate { get; private set; }
        public EntityGame(string title, string genre, DateTime releaseDate)
        {
            Title = title;
            Genre = genre;
            ReleaseDate = releaseDate;
        }
    }
}
