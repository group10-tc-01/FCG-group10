using System.Net;

namespace FCG.Domain.Exceptions
{
    public class ConflictException : ApiException
    {
        public ConflictException(string message) : base(HttpStatusCode.Conflict, message) { }
        public ConflictException(string message, Exception innerException) : base(HttpStatusCode.Conflict, message, innerException) { }

    }
}
