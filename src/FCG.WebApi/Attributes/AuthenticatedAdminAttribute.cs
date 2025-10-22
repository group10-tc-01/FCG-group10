using FCG.WebApi.Filter;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace FCG.WebApi.Attributes
{
    [ExcludeFromCodeCoverage]
    public class AuthenticatedAdminAttribute : TypeFilterAttribute
    {
        public AuthenticatedAdminAttribute() : base(typeof(AuthenticatedAdminFilter))
        {
        }
    }
}
