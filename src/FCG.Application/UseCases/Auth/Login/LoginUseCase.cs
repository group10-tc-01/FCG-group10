using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;

namespace FCG.Application.UseCases.Auth.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly ITokenService _tokenService;

        public LoginUseCase(IReadOnlyUserRepository readOnlyUserRepository, ITokenService tokenService)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginOutput> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            var user = await _readOnlyUserRepository.GetByEmailAndPasswordAsync(request.Email, request.Password);

            if (user is null)
                throw new UnauthorizedException("Invalid email or password.");

            var accessToken = _tokenService.GenerateAccessToken(user!);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();
            var refreshToken = await _tokenService.SaveRefreshTokenAsync(refreshTokenValue, user!.Id);

            return new LoginOutput
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = 3600
            };
        }
    }
}
