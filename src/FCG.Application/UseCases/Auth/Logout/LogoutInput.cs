using MediatR;

namespace FCG.Application.UseCases.Auth.Logout
{
    public class LogoutInput : IRequest<LogoutOutput>
    {
        public Guid UserId { get; init; }
    }
}