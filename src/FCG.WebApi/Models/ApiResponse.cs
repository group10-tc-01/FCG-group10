using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace FCG.WebApi.Models
{
    [ExcludeFromCodeCoverage]
    public class ApiResponse<T>
    {
        public bool Succes { get; set; }
        public T Data { get; set; } = default!;
        public List<string> ErrorMessages { get; set; } = default!;

        public static ApiResponse<T> SuccesResponse(T data)
        {
            return new ApiResponse<T> { Succes = true, Data = data };
        }

        public static ApiResponse<T> ErrorResponse(List<string> errorMessages, HttpStatusCode statusCode)
        {
            return new ApiResponse<T>
            {
                Succes = false,
                ErrorMessages = errorMessages,
            };
        }
    }
}
