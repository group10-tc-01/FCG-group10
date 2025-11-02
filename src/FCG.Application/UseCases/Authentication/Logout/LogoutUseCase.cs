using FCG.Domain.Repositories.RefreshTokenRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Authentication.Logout
{
    public class LogoutUseCase : ILogoutUseCase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<LogoutUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public LogoutUseCase(
            IRefreshTokenRepository refreshTokenRepository,
            ILogger<LogoutUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<LogoutOutput> Handle(LogoutInput request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[LogoutUseCase] [CorrelationId: {CorrelationId}] Logging out user: {UserId}",
                correlationId, request.UserId);

            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId);

            _logger.LogInformation(
                "[LogoutUseCase] [CorrelationId: {CorrelationId}] Successfully logged out user: {UserId}",
                correlationId, request.UserId);

            return new LogoutOutput
            {
                Message = ResourceMessages.LogoutSuccessFull
            };
        }
    }
}