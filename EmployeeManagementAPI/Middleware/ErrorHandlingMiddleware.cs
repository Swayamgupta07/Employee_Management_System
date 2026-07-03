using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using EmployeeManagementAPI.Models.Data;

namespace EmployeeManagementAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught by global error handling middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                TraceId = context.TraceIdentifier
            };

            switch (exception)
            {
                case OperationCanceledException canceledEx:
                    response.Message = "The request was cancelled by the client.";
                    response.StatusCode = 499;
                    response.ErrorType = "RequestCancelled";
                    context.Response.StatusCode = response.StatusCode;
                    _logger.LogWarning(canceledEx, "Request was cancelled");
                    break;

                case ArgumentNullException argNullEx:
                    response.Message = argNullEx.Message;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ErrorType = "ValidationError";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(argNullEx, "Null argument error occurred");
                    break;

                case ArgumentException argEx:
                    response.Message = argEx.Message;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ErrorType = "ValidationError";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(argEx, "Validation error occurred");
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    response.Message = keyNotFoundEx.Message;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.ErrorType = "NotFound";
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogWarning(keyNotFoundEx, "Resource not found error occurred");
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    response.Message = "Access denied";
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.ErrorType = "Unauthorized";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogWarning(unauthorizedEx, "Unauthorized access error occurred");
                    break;

                case DbUpdateConcurrencyException concurrencyEx:
                    response.Message = "The record was modified by another user. Please refresh and try again.";
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.ErrorType = "ConcurrencyError";
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    _logger.LogError(concurrencyEx, "Database concurrency error occurred");
                    break;

                case DbUpdateException dbEx:
                    response.Message = "A database error occurred";
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ErrorType = "DatabaseError";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(dbEx, "Database update error occurred");
                    break;

                case TimeoutException timeoutEx:
                    response.Message = "The operation timed out. Please try again.";
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    response.ErrorType = "TimeoutError";
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    _logger.LogError(timeoutEx, "Timeout error occurred");
                    break;

                default:
                    response.Message = _environment.IsDevelopment()
                        ? exception.Message
                        : "An internal server error occurred";
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ErrorType = "InternalServerError";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(exception, "Unhandled exception occurred");
                    break;
            }

            if (_environment.IsDevelopment())
            {
                response.Details = exception.StackTrace;
                response.InnerException = exception.InnerException?.Message;
            }

            response.Timestamp = DateTime.UtcNow;
            response.Path = context.Request.Path;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}