using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace FCG.Domain.Exceptions
{
    [ExcludeFromCodeCoverage(Justification = "Will be implemented later")]
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) { }
        public NotFoundException(string message, Exception innerException) : base(HttpStatusCode.NotFound, message, innerException) { }
    }
}
