using MediatR;

namespace FCG.Application.UseCases.Users.MyGames
{
    public interface ILibraryGameUseCase : IRequestHandler<LibraryGameUseCaseRequest, ICollection<LibraryGameResponse>>
    {
    }
}
