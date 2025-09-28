using FCG.Domain.Repositories.RefreshTokenRepository;

namespace FCG.Application.UseCases.Authentication.Logout
{
    public class LogoutUseCase : ILogoutUseCase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutUseCase(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LogoutOutput> Handle(LogoutInput request, CancellationToken cancellationToken)
        {
            await _refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId);

            return new LogoutOutput
            {
                Message = "Logout successful. All refresh tokens have been revoked.",
                Success = true
            };
        }
    }
}