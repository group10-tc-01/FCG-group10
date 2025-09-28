using MediatR;

namespace FCG.Application.UseCases.Authentication.Logout
{
    public interface ILogoutUseCase : IRequestHandler<LogoutInput, LogoutOutput> { }
}