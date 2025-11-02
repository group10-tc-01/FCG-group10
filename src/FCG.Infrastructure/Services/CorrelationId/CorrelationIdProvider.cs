using FCG.Domain.Services;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Infrastructure.Services.CorrelationId
{
    [ExcludeFromCodeCoverage]
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CorrelationIdKey = "CorrelationId";

        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCorrelationId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.Items.ContainsKey(CorrelationIdKey) == true)
            {
                return httpContext.Items[CorrelationIdKey]?.ToString() ?? string.Empty;
            }

            return string.Empty;
        }
    }
}
