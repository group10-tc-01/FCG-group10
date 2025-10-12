using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FCG.WebApi.Filter
{
    public class AuthenticatedAdminFilter : IAsyncAuthorizationFilter
    {
        private readonly ITokenService _tokenService;
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;

        public AuthenticatedAdminFilter(ITokenService tokenService, IReadOnlyUserRepository readOnlyUserRepository)
        {
            _tokenService = tokenService;
            _readOnlyUserRepository = readOnlyUserRepository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var token = GetToken(context);

            var userId = _tokenService.ValidateAccessToken(token);

            var user = await _readOnlyUserRepository.GetByIdAsync(userId) ?? throw new UnauthorizedException(ResourceMessages.InvalidToken);

            if (user.Role != Role.Admin)
            {
                throw new ForbiddenAccessException(ResourceMessages.InvalidAccessLevel);
            }
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
