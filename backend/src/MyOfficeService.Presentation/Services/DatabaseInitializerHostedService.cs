using MyOfficeService.Infrastructure.Postgres.Migrations;

namespace MyOfficeService.Presentation.Services;

/// <summary>
/// DatabaseInitializerHostedService - запускается 1 раз при старте приложения, нужен для того,
/// чтобы dbup добавил миграции.
/// </summary>
public class DatabaseInitializerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializerHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        dbInitializer.Initialize();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}