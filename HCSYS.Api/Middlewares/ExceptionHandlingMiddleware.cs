using HCSYS.Api.Models;
using HCSYS.Core.Exceptions;

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
                UnprocessableEntityException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError,
            };

            switch (exception)
            {
                case UnprocessableEntityException unprocessableEntityException:
                    await HandleUnprocessableEntityExceptionAsync(context, unprocessableEntityException);
                    break;
                default:
                    await HandleByDefaultAsync(context, exception);
                    break;
            }
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

    private async Task HandleUnprocessableEntityExceptionAsync(HttpContext context, UnprocessableEntityException exception)
    {
        await context.Response.WriteAsJsonAsync(new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Title = exception.Message,
            Errors = exception.Errors,
        });
    }
}