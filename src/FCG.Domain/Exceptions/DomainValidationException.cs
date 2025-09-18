using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace FCG.Domain.Exceptions
{
    [ExcludeFromCodeCoverage(Justification = "Will be implemented later")]
    public class DomainValidationException : ApiException
    {
        public DomainValidationException(string message) : base(HttpStatusCode.BadRequest, message) { }
        public DomainValidationException(string message, Exception innerException) : base(HttpStatusCode.BadRequest, message, innerException) { }

    }
}