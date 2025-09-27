using MediatR;

namespace FCG.Application.UseCases.Auth.Logout
{
    public interface ILogoutUseCase : IRequestHandler<LogoutInput, LogoutOutput> { }
}