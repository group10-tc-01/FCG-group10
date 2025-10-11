using FCG.WebApi.Filter;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebApi.Attributes
{
    public class AuthenticatedAdminAttribute : TypeFilterAttribute
    {
        public AuthenticatedAdminAttribute() : base(typeof(AuthenticatedAdminFilter)) { }
    }
}
