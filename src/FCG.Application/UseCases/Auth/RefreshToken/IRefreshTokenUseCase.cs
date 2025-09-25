using MediatR;

namespace FCG.Application.UseCases.Auth.RefreshToken
{
    public interface IRefreshTokenUseCase : IRequestHandler<RefreshTokenInput, RefreshTokenOutput> { }
}