using BuildingBlock.Behaviors;
using BuildingBlock.Exceptions.Handlers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMarten(builder.Configuration.GetConnectionString("Database")).UseLightweightSessions();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
var app = builder.Build();
app.MapCarter();
app.UseExceptionHandler(options => { });
/*app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        if (exception is null)
        {
            return;
        }
        switch (exception)
        {
            case ValidationException validationException:
                var error = ApplicationError.ValidationError(validationException.Message);
                var problemDetails = new ProblemDetails
                {
                    Title = error.Code,
                    Detail = error.Description,
                    Status = StatusCodes.Status400BadRequest
                };
                context.Response.StatusCode = problemDetails.Status.Value;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problemDetails);
                break;
            default:
                var problem = new ProblemDetails
                {
                    Title = "InternalServerError",
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError
                };
                context.Response.StatusCode = problem.Status.Value;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
                break;
        }
    });
}); */
app.Run();
