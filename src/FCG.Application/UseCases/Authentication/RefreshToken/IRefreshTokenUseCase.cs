using MediatR;

namespace FCG.Application.UseCases.Authentication.RefreshToken
{
    public interface IRefreshTokenUseCase : IRequestHandler<RefreshTokenInput, RefreshTokenOutput> { }
}