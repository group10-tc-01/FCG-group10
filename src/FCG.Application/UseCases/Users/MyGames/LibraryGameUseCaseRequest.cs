using MediatR;

namespace FCG.Application.UseCases.Users.MyGames
{
    public class LibraryGameUseCaseRequest : IRequest<ICollection<LibraryGameResponse>>
    {
    }
}
