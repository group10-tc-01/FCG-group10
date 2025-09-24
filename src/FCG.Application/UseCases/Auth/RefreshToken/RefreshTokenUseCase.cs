namespace FCG.Application.UseCases.Auth.RefreshToken
{
    //public class RefreshTokenUseCase : IRefreshTokenUseCase
    //{
    //    private readonly ITokenService _tokenService;
    //    private readonly IReadOnlyUserRepository _userRepository;

    //    public RefreshTokenUseCase(ITokenService tokenService, IReadOnlyUserRepository userRepository)
    //    {
    //        _tokenService = tokenService;
    //        _userRepository = userRepository;
    //    }

    //    public async Task<RefreshTokenOutput> Handle(RefreshTokenInput request, CancellationToken cancellationToken)
    //    {
    //        var userId = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);

    //        if (userId is null)
    //            throw new UnauthorizedException("Invalid refresh token.");

    //        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));

    //        if (user is null)
    //            throw new UnauthorizedException("User not found.");

    //        await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

    //        var accessToken = _tokenService.GenerateAccessToken(user);
    //        var newRefreshTokenValue = _tokenService.GenerateRefreshToken();
    //        var newRefreshToken = await _tokenService.SaveRefreshTokenAsync(newRefreshTokenValue, user.Id);

    //        return new RefreshTokenOutput
    //        {
    //            AccessToken = accessToken,
    //            RefreshToken = newRefreshToken.Token,
    //            ExpiresIn = 3600
    //        };
    //    }
    //}
}