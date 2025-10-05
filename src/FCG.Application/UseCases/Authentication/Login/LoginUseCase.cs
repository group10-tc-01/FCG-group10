using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Models.Authenticaiton;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.Extensions.Options;

namespace FCG.Application.UseCases.Authentication.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordEncrypter _passwordEncrypter;

        public LoginUseCase(IReadOnlyUserRepository readOnlyUserRepository, ITokenService tokenService,
            IOptions<JwtSettings> jwtSettings, IPasswordEncrypter passwordEncrypter)
        {
            _tokenService = tokenService;
            _readOnlyUserRepository = readOnlyUserRepository;
            _jwtSettings = jwtSettings.Value;
            _passwordEncrypter = passwordEncrypter;
        }

        public async Task<LoginOutput> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            var user = await _readOnlyUserRepository.GetByEmailAsync(request.Email, cancellationToken);

            ValidateUser(request, user);

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

        private void ValidateUser(LoginInput request, User? user)
        {
            if (user is null)
                throw new UnauthorizedException(ResourceMessages.InvalidEmailOrPassword);

            var isPasswordValid = _passwordEncrypter.IsValid(request.Password, user.Password.Value);

            if (isPasswordValid is false)
                throw new UnauthorizedException(ResourceMessages.InvalidEmailOrPassword);
        }
    }
}
