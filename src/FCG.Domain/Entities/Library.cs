using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;

namespace FCG.Domain.Entities
{
    public sealed class Library : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User? User { get; }
        private readonly List<LibraryGame> _libraryGames;
        public IReadOnlyCollection<LibraryGame> LibraryGames => _libraryGames.AsReadOnly();

        public Library(Guid userId)
        {
            UserId = userId;
            _libraryGames = new List<LibraryGame>();
        }

        public static Library Create(Guid userId)
        {
            return new Library(userId);
        }

        public void AddGame(Guid gameId, Price purchasePrice)
        {
            var gameAlreadyExists = _libraryGames.Any(lg => lg.GameId == gameId);

            if (gameAlreadyExists)
            {
                throw new DomainException($"Game with Id {gameId} is already in the library.");
            }

            var libraryGame = LibraryGame.Create(Id, gameId, purchasePrice);

            _libraryGames.Add(libraryGame);
        }
    }
}
