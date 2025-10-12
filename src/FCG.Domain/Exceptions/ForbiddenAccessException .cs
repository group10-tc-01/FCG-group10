using System.Net;

namespace FCG.Domain.Exceptions
{
    public class ForbiddenAccessException : ApiException
    {
        public ForbiddenAccessException(string message) : base(HttpStatusCode.Forbidden, message) { }
        public ForbiddenAccessException(string message, Exception innerException) : base(HttpStatusCode.Unauthorized, message, innerException) { }
    }
}
