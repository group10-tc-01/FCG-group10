﻿using FCG.Domain.Entities;
using FCG.Domain.Services;
using FCG.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

            var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

            return await _fcgDbContext.Users.AsNoTracking().FirstAsync(user => user.Id == userId);
        }
    }
}
