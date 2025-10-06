using MediatR;

namespace FCG.Application.UseCases.Authentication.RefreshToken
{
    public class RefreshTokenInput : IRequest<RefreshTokenOutput>
    {
        public string RefreshToken { get; init; } = string.Empty;
    }
}