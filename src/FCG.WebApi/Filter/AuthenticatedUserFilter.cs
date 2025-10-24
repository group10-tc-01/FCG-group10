using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FCG.WebApi.Filter
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        private readonly ITokenService _tokenService;
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly ICurrentUserService _currentUserService;

        public AuthenticatedUserFilter(ITokenService tokenService, IReadOnlyUserRepository readOnlyUserRepository, ICurrentUserService currentUserService)
        {
            _tokenService = tokenService;
            _readOnlyUserRepository = readOnlyUserRepository;
            _currentUserService = currentUserService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var token = GetToken(context);

            var userId = _tokenService.ValidateAccessToken(token);

            var user = await _readOnlyUserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UnauthorizedException(ResourceMessages.InvalidToken);
            }

            _currentUserService.UserId = userId;
        }

        private static string GetToken(AuthorizationFilterContext context)
        {
            var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(authentication))
            {
                throw new UnauthorizedException(ResourceMessages.InvalidToken);
            }

            if (!authentication.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedException(ResourceMessages.InvalidToken);
            }

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
