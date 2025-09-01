using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyOfficeService.Application.Abstractions.Database;
using MyOfficeService.Application.Repositories;
using MyOfficeService.Infrastructure.Postgres.Migrations;
using MyOfficeService.Infrastructure.Postgres.Repositories;

namespace MyOfficeService.Infrastructure.Postgres.DependencyInjection;

/// <summary>
/// DependencyInjection в слое Infrastructure добавляет все нужные зависимости именно Infrastructure слоя, после чего
/// в Presentation слое можно добавить весь инфраструктурный слой через AddInfrastructurePostgresLayer
/// </summary>
public static class DependencyInjection
{
    private const string POSTGRES_CONNECTION_STRING_KEY = "Postgres_ConnectionString";

    /// <summary>
    /// AddInfrastructurePostgresLayer - собираю все зависимости
    /// </summary>
    public static void AddInfrastructurePostgresLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration[POSTGRES_CONNECTION_STRING_KEY]
                               ?? throw new InvalidOperationException("Postgres connection string is null");

        services.AddMigrations(connectionString);
        services.AddRepositories();
        services.AddDapperDbContext(connectionString);
    }

    /// <summary>
    /// AddRepositories - подключаю все репозитории
    /// </summary>
    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
    }

    /// <summary>
    /// AddMigrations - добавляю миграции
    /// </summary>
    private static void AddMigrations(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<DatabaseInitializer>(_ => new DatabaseInitializer(connectionString));
    }

    /// <summary>
    /// AddDapperDbContext - подключаю DapperDbContext и TransactionManager
    /// </summary>
    private static void AddDapperDbContext(this IServiceCollection services, string connectionString)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<DapperDbContext>(_ => new DapperDbContext(connectionString));
        services.AddScoped<ITransactionManager, TransactionManager>();
    }
}