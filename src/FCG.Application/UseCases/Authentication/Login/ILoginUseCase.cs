using MediatR;

namespace FCG.Application.UseCases.Authentication.Login
{
    public interface ILoginUseCase : IRequestHandler<LoginInput, LoginOutput> { }
}
