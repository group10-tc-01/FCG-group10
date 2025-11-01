using MediatR;

namespace FCG.Application.UseCases.Users.MyGames.AddGameToLibrary
{
    public class AddGameToLibraryRequest : IRequest<LibraryGameResponse>
    {
        public Guid GameId { get; set; }
    }
}
