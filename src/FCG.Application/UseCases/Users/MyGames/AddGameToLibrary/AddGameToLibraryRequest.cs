using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.Users.MyGames.AddGameToLibrary
{
    public class AddGameToLibraryRequest : IRequest<LibraryGameResponse>
    {
        [ExcludeFromCodeCoverage]
        public Guid GameId { get; set; }
    }
}
