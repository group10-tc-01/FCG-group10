using FCG.Domain.Services;
using System.Diagnostics.CodeAnalysis;

namespace FCG.WebApi.Context
{
    [ExcludeFromCodeCoverage]
    public class ContextTokenValue : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextTokenValue(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            var authentication = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
