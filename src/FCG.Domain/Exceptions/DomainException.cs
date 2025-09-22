using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace FCG.Domain.Exceptions
{

    [ExcludeFromCodeCoverage(Justification = "Will be implemented later")]
    public class DomainException : ApiException
    {
        public DomainException(string message) : base(HttpStatusCode.BadRequest, message) { }
        public DomainException(string message, Exception innerException) : base(HttpStatusCode.BadRequest, message, innerException) { }
    }
}