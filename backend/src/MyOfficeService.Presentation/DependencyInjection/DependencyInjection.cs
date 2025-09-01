using Microsoft.OpenApi.Models;
using MyOfficeService.Application.DependencyInjection;
using MyOfficeService.Infrastructure.Postgres.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace MyOfficeService.Presentation.DependencyInjection;

/// <summary>
/// DependencyInjection - собираю все зависимоти приложения
/// </summary>
public static class DependencyInjection
{
    public static void ConfigurationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLayers(configuration);
        services.AddSwagger();
        services.AddLogging();
    }

    /// <summary>
    /// AddLogging - зависимости логгирования
    /// </summary>
    private static void AddLogging(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Destructure.ToMaximumDepth(4)
            .WriteTo.Console()
            .CreateLogger();

        services.AddSerilog();
    }

    /// <summary>
    /// AddSwagger - зависимости swagger
    /// </summary>
    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v",
                new OpenApiInfo
                {
                    Version = "v",
                    Title = "OfficeService API",
                    Description = "OfficeService тестовое задание",
                    Contact = new OpenApiContact { Name = "Торак", Url = new Uri("https://t.me/Tor_ak") }
                });
        });
    }

    /// <summary>
    /// AddLayers - добавление всех слоёв приложения
    /// </summary>
    private static void AddLayers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructurePostgresLayer(configuration);
        services.AddApplicationLayer();
    }
}