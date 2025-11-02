using System.Net;

namespace FCG.Domain.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message) { }
        public BadRequestException(string message, Exception innerException) : base(HttpStatusCode.BadRequest, message, innerException) { }

    }
}
