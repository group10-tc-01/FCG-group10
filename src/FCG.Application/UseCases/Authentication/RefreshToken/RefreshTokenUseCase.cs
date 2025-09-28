using FCG.Domain.Exceptions;
using FCG.Domain.Models.Authenticaiton;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using Microsoft.Extensions.Options;

namespace FCG.Application.UseCases.Authentication.RefreshToken
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly ITokenService _tokenService;
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenUseCase(ITokenService tokenService, IReadOnlyUserRepository readOnlyUserRepository, IOptions<JwtSettings> jwtSettings)
        {
            _tokenService = tokenService;
            _readOnlyUserRepository = readOnlyUserRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<RefreshTokenOutput> Handle(RefreshTokenInput request, CancellationToken cancellationToken)
        {
            var userId = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);

            if (userId is null)
                throw new UnauthorizedException("Invalid refresh token.");

            var user = await _readOnlyUserRepository.GetByIdAsync(Guid.Parse(userId));

            if (user is null)
                throw new UnauthorizedException("User not found.");

            await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshTokenValue = _tokenService.GenerateRefreshToken();
            var newRefreshToken = await _tokenService.SaveRefreshTokenAsync(newRefreshTokenValue, user.Id);

            return new RefreshTokenOutput
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresInDays = _jwtSettings.RefreshTokenExpirationDays
            };
        }
    }
}