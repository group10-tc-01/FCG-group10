using System.Diagnostics.CodeAnalysis;
using FCG.WebApi.Filter;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Attributes
{
    [ExcludeFromCodeCoverage]
    public class AuthenticatedAdminAttribute : TypeFilterAttribute
    {
        public AuthenticatedAdminAttribute() : base(typeof(AuthenticatedAdminFilter)) { }
    }
}
