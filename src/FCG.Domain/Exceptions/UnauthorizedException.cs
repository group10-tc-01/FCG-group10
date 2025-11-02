using System.Net;

namespace FCG.Domain.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, message) { }
        public UnauthorizedException(string message, Exception innerException) : base(HttpStatusCode.Unauthorized, message, innerException) { }
    }
}
