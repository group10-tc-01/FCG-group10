using MediatR;

namespace FCG.Application.UseCases.Authentication.Logout
{
    public class LogoutInput : IRequest<LogoutOutput>
    {
        public Guid UserId { get; init; }
    }
}