using System.Net;

namespace FCG.Domain.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) { }
        public NotFoundException(string message, Exception innerException) : base(HttpStatusCode.NotFound, message, innerException) { }
    }
}
