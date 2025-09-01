using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;
using MyOfficeService.Presentation.Responses;

namespace MyOfficeService.Presentation.Middlewares;

/// <summary>
/// ExceptionMiddleware - глобальный обработчик исключений
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await ExceptionHandlerAsync(context, ex);
        }
    }

    private async Task ExceptionHandlerAsync(HttpContext context, Exception exception)
    {
        var (error, statusCode) = GetErrorWithStatusCode(exception);
        var envelope = Envelope<ErrorList>.Error(error.ToErrorList());

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;


        await context.Response.WriteAsJsonAsync(envelope);
    }

    private (Error, int) GetErrorWithStatusCode(Exception ex)
    {
        return ex switch
        {
            _ =>
                (GeneralErrors.Server.ServerError("Неизвестная ошибка"),
                    (int)StatusCodes.Status500InternalServerError)
        };
    }
}