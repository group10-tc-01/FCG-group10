using FCG.WebApi.Filter;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Attributes
{
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter)) { }
    }
}
