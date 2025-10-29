using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Models.Authentication;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FCG.Application.UseCases.Authentication.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly ILogger<LoginUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public LoginUseCase(
            IReadOnlyUserRepository readOnlyUserRepository, 
            ITokenService tokenService,
            IOptions<JwtSettings> jwtSettings, 
            IPasswordEncrypter passwordEncrypter,
            ILogger<LoginUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _tokenService = tokenService;
            _readOnlyUserRepository = readOnlyUserRepository;
            _jwtSettings = jwtSettings.Value;
            _passwordEncrypter = passwordEncrypter;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<LoginOutput> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();
            
            _logger.LogInformation("[LoginUseCase] [CorrelationId: {CorrelationId}] Starting login process for email: {Email}", 
                correlationId, request.Email);

            var user = await _readOnlyUserRepository.GetByEmailAsync(request.Email, cancellationToken);

            ValidateUser(request, user);

            var accessToken = _tokenService.GenerateAccessToken(user!);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();
            var refreshToken = await _tokenService.SaveRefreshTokenAsync(refreshTokenValue, user!.Id);

            _logger.LogInformation("[LoginUseCase] [CorrelationId: {CorrelationId}] Login successful for user: {UserId}", 
                correlationId, user.Id);

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
