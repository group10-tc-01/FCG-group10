using System.Net;

namespace FCG.Domain.Exceptions
{
    public class DuplicateEmailException : ApiException
    {
        public DuplicateEmailException(string message) : base(HttpStatusCode.Conflict, message) { }
        public DuplicateEmailException(string message, Exception innerException) : base(HttpStatusCode.Conflict, message, innerException) { }

    }
}
