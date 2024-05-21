using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace BuildingBlock.Exceptions.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception has occurred: {Message} at time {Time}", exception.Message, DateTime.UtcNow);
            (string Detail, string Title, string StatusCode) = exception switch
            {
                ValidationException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest.ToString()),
                _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError.ToString())
            };
            var problemDetails = new ProblemDetails
            {
                Title = Title,
                Detail = Detail,
                Status = int.Parse(StatusCode),
                Instance = httpContext.Request.Path
            };
            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
            return true;
        }
    }
}
