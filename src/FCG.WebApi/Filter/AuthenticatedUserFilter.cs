using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace FCG.WebApi.Filter
{
    [ExcludeFromCodeCoverage]
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        private readonly ITokenService _tokenService;
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;

        public AuthenticatedUserFilter(ITokenService tokenService, IReadOnlyUserRepository readOnlyUserRepository)
        {
            _tokenService = tokenService;
            _readOnlyUserRepository = readOnlyUserRepository;
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
        }

        private static string GetToken(AuthorizationFilterContext context)
        {
            var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(authentication))
            {
                throw new UnauthorizedException(ResourceMessages.InvalidToken);
            }

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
