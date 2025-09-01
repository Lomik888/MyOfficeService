using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyOfficeService.Application.Abstractions.Handlers;

namespace MyOfficeService.Application.DependencyInjection;

/// <summary>
/// DependencyInjection в слое Application добавляет все нужные зависимости именно Application слоя, после чего
/// в Presentation слое можно добавить весь слой бизнес логики через AddApplicationLayer
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// AddApplicationLayer - собираю все зависимости
    /// </summary>
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddValidators();
        services.AddCommands();
        services.AddQueries();
    }

    /// <summary>
    /// AddCommands - подключаю все команды через scrutor
    /// </summary>
    private static void AddCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyDependencies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(ICommandHandler<,,>), typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces());
    }

    /// <summary>
    /// AddQueries - подключаю все запросы через scrutor
    /// </summary>
    private static void AddQueries(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyDependencies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(IQueryHandler<,,>), typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces());
    }

    /// <summary>
    /// AddValidators - подключаю все валидаторы из FluentValidation
    /// </summary>
    private static void AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
    }
}