using MediatR;

namespace FCG.Application.UseCases.Auth.Login
{
    public class LoginInput : IRequest<LoginOutput>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
