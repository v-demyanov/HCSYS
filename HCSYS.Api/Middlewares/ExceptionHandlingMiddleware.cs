using HCSYS.Api.Models;
using HCSYS.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace HCSYS.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = exception switch
            {
                EntityNotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError,
            };

            await this.HandleByDefaultAsync(context, exception);
        }
    }

    private async Task HandleByDefaultAsync(HttpContext context, Exception exception)
    {
        await context.Response.WriteAsJsonAsync(new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Title = exception.Message,
        });
    }
}