using FCG.Domain.Exceptions;
using FCG.Domain.Models.Authentication;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FCG.Application.UseCases.Authentication.RefreshToken
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly ITokenService _tokenService;
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<RefreshTokenUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public RefreshTokenUseCase(
            ITokenService tokenService, 
            IReadOnlyUserRepository readOnlyUserRepository, 
            IOptions<JwtSettings> jwtSettings,
            ILogger<RefreshTokenUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _tokenService = tokenService;
            _readOnlyUserRepository = readOnlyUserRepository;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<RefreshTokenOutput> Handle(RefreshTokenInput request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[RefreshTokenUseCase] [CorrelationId: {CorrelationId}] Validating refresh token",
                correlationId);

            var userId = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);

            if (userId is null)
            {
                _logger.LogWarning(
                    "[RefreshTokenUseCase] [CorrelationId: {CorrelationId}] Invalid refresh token",
                    correlationId);

                throw new UnauthorizedException(ResourceMessages.InvalidRefreshToken);
            }

            var user = await _readOnlyUserRepository.GetByIdAsync(Guid.Parse(userId));

            if (user is null)
            {
                _logger.LogWarning(
                    "[RefreshTokenUseCase] [CorrelationId: {CorrelationId}] User not found for userId: {UserId}",
                    correlationId, userId);

                throw new UnauthorizedException(ResourceMessages.InvalidRefreshToken);
            }

            await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshTokenValue = _tokenService.GenerateRefreshToken();
            var newRefreshToken = await _tokenService.SaveRefreshTokenAsync(newRefreshTokenValue, user.Id);

            _logger.LogInformation(
                "[RefreshTokenUseCase] [CorrelationId: {CorrelationId}] Successfully refreshed token for user: {UserId}",
                correlationId, user.Id);

            return new RefreshTokenOutput
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresInDays = _jwtSettings.RefreshTokenExpirationDays
            };
        }
    }
}