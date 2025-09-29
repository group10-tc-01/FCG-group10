using FCG.Domain.Repositories.RefreshTokenRepository;
using FCG.Messages;

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
                Message = ResourceMessages.LogoutSuccessFull,
                Success = true
            };
        }
    }
}