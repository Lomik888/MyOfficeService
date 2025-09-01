using CSharpFunctionalExtensions;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Abstractions.Handlers;

/// <summary>
/// IQueryHandler - для имплементации в Handler.
/// Является контрактом на асинхронный метод возвращающий Result TResult, TError
/// </summary>
/// <typeparam name="TResultValue">Что возращает Handler</typeparam>
/// <typeparam name="TResultError">Какой тип ошибки при неудачном вызову</typeparam>
/// <typeparam name="TQuery">Запрос, для которой был реализован Handler</typeparam>
public interface IQueryHandler<TResultValue, TResultError, in TQuery>
    where TQuery : IQuery
    where TResultError : ErrorList
{
    public Task<Result<TResultValue, TResultError>> HandleAsync(
        TQuery request,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// IQueryHandler - для имплементации в Handler.
/// Является контрактом на асинхронный метод возвращающий Result TResultError
/// </summary>
/// <typeparam name="TResultError">Какой тип ошибки при неудачном вызову</typeparam>
/// <typeparam name="TQuery">Запрос, для которой был реализован Handler</typeparam>
public interface IQueryHandler<TResultError, in TQuery>
    where TQuery : IQuery
    where TResultError : ErrorList
{
    public Task<UnitResult<TResultError>> HandleAsync(
        TQuery request,
        CancellationToken cancellationToken = default);
}