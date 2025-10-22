using MediatR;

namespace FCG.Application.UseCases.Games.Register
{
    public interface IRegisterGameUseCase : IRequestHandler<RegisterGameInput, RegisterGameOutput> { }
}
