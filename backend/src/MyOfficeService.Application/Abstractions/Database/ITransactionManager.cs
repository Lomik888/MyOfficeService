using CSharpFunctionalExtensions;
using MyOfficeService.Application.Abstractions.Enums;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Abstractions.Database;

public interface ITransactionManager : IAsyncDisposable
{
    Task<Result<ITransaction, Error>> BeginTransactionAsync(
        CancellationToken cancellationToken,
        TransactionIsolationLevel transactionIsolationLevel = TransactionIsolationLevel.READ_COMMITTED);
}