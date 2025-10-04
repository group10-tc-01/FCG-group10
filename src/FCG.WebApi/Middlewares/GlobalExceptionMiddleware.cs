using FCG.Domain.Exceptions;
using FCG.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace FCG.WebApi.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }

        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = context!.TraceIdentifier;

            context!.Response.ContentType = "application/json";

            if (exception is ApiException apiException)
            {
                await HandleApiExceptionAsync(context, apiException);
                return;
            }

            await HandleGenericExceptionAsync(context, exception, traceId);
        }

        private async Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
        {
            context.Response.StatusCode = (int)exception.StatusCode;

            var response = ApiResponse<object>.ErrorResponse(new List<string> { exception.Message }, exception.StatusCode);

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private async Task HandleGenericExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Title = "An unexpected error occurred!",
                Status = context.Response.StatusCode,
                Instance = context.Request.Path,
                Detail = "Please contact support.",
            };

            problemDetails.Extensions["traceId"] = traceId;

            if (_env.IsDevelopment())
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            var jsonResponse = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
