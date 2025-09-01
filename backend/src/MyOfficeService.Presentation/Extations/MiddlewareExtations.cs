using MyOfficeService.Presentation.Middlewares;

namespace MyOfficeService.Presentation.Extations;

public static class MiddlewareExtations
{
    public static IApplicationBuilder UseGlobalExceptionsHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}