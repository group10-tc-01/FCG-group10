using FCG.Domain.Exceptions;
using FCG.Domain.Models.Authenticaiton;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using Microsoft.Extensions.Options;

namespace FCG.Application.UseCases.Authentication.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public LoginUseCase(IReadOnlyUserRepository readOnlyUserRepository, ITokenService tokenService, IOptions<JwtSettings> jwtSettings)
        {
            _tokenService = tokenService;
            _readOnlyUserRepository = readOnlyUserRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginOutput> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            //TODO: Alterar chamada para fazer decrypt da password
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
                ExpiresInMinutes = _jwtSettings.AccessTokenExpirationMinutes
            };
        }
    }
}
