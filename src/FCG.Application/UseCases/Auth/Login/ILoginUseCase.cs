using MediatR;

namespace FCG.Application.UseCases.Auth.Login
{
    public interface ILoginUseCase : IRequestHandler<LoginInput, LoginOutput> { }
}
