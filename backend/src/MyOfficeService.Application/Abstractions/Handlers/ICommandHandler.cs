using CSharpFunctionalExtensions;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Abstractions.Handlers;

/// <summary>
/// ICommandHandler - для имплементации в Handler.
/// Является контрактом на асинхронный метод возвращающий Result<TResult, TError>
/// </summary>
/// <typeparam name="TResult">Что возращает Handler</typeparam>
/// <typeparam name="TError">Какой тип ошибки при неудачном вызову</typeparam>
/// <typeparam name="TCommand">Команда, для которой был реализован Handler</typeparam>
public interface ICommandHandler<TResult, TError, TCommand>
    where TCommand : ICommand
    where TError : ErrorList
{
    public Task<Result<TResult, TError>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken);
}

/// <summary>
/// ICommandHandler - для имплементации в Handler.
/// Является контрактом на асинхронный метод возвращающий UnitResult<TError>
/// </summary>
/// <typeparam name="TError">Какой тип ошибки при неудачном вызову</typeparam>
/// <typeparam name="TCommand">Команда, для которой был реализован Handler</typeparam>
public interface ICommandHandler<TError, TCommand>
    where TCommand : ICommand
    where TError : ErrorList
{
    public Task<UnitResult<TError>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken);
}