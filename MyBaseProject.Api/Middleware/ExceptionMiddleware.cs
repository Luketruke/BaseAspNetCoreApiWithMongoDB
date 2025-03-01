using FluentValidation;
using MongoDB.Driver;
using System.Net;
using MyBaseProject.Domain.Exceptions;

namespace MyBaseProject.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                await HandleFluentValidationExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleMiddlewareExceptionAsync(context, ex);
            }
        }
        private static Task HandleFluentValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = exception.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return context.Response.WriteAsJsonAsync(new { message = "Validation Failed", errors });
        }
        private Task HandleMiddlewareExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                InvalidCredentialsException => ((int)HttpStatusCode.Unauthorized, exception.Message),
                UserNotFoundException => ((int)HttpStatusCode.Unauthorized, exception.Message),
                EmailAlreadyExistsException => ((int)HttpStatusCode.BadRequest, exception.Message),
                RequiredPasswordException => ((int)HttpStatusCode.BadRequest, exception.Message),
                InvalidEmailFormatException => ((int)HttpStatusCode.BadRequest, exception.Message),
                FormatException => ((int)HttpStatusCode.BadRequest, exception.Message),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                ErrorWhileProcessingException => ((int)HttpStatusCode.Conflict, exception.Message),
                DatabaseOperationException => ((int)HttpStatusCode.InternalServerError, "A database error occurred."),
                DatabaseWriteException => ((int)HttpStatusCode.InternalServerError, "A database write error occurred."),
                MongoException => ((int)HttpStatusCode.InternalServerError, "A database error occurred."),
                MessageNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                EntityNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            var result = new
            {
                message,
                Details = statusCode == (int)HttpStatusCode.InternalServerError ? exception.StackTrace : null
            };

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(result);
        }
    }
}
