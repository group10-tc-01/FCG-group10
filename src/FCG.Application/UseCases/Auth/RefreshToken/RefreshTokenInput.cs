using MediatR;

namespace FCG.Application.UseCases.Auth.RefreshToken
{
    public class RefreshTokenInput : IRequest<RefreshTokenOutput>
    {
        public string RefreshToken { get; init; } = string.Empty;
    }
}