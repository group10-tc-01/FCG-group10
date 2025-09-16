using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace FCG.Domain.Exceptions
{
    [ExcludeFromCodeCoverage(Justification = "Will be implemented later")]
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        protected ApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        protected ApiException(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
