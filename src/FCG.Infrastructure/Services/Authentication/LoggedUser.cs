using FCG.Domain.Entities;
using FCG.Domain.Services;
using FCG.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FCG.Domain.Exceptions;

namespace FCG.Infrastructure.Services.Authentication
{
    [ExcludeFromCodeCoverage]
    public class LoggedUser : ILoggedUser
    {
        private readonly FcgDbContext _fcgDbContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(FcgDbContext fcgDbContext, ITokenProvider tokenProvider)
        {
            _fcgDbContext = fcgDbContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> GetLoggedUserAsync()
        {
            var token = _tokenProvider.GetToken();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userIdValue = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
            {
                throw new UnauthorizedException("Invalid user identifier in token.");
            }

            var user = await _fcgDbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
            {
                throw new UnauthorizedException("User not found.");
            }

            return user;
        }
    }
}
