using System.Data.Common;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Abstractions.Database;

namespace MyOfficeService.Infrastructure.Postgres;

/// <summary>
/// Transaction - имплементация ITransaction - обёртки для работы с транзакцией.
/// Транзакцию можно Commit, Rollback и Dispose.
/// Подключение к БД открывается само, когда TransactionManager возвращает ITransaction
/// </summary>
public class Transaction : ITransaction
{
    private readonly DbTransaction _transaction;
    private readonly ILogger<Transaction> _logger;
    private readonly int _connectionId;

    public Transaction(DbTransaction transaction, ILoggerFactory loggerFactory, int connectionId)
    {
        _transaction = transaction;
        _connectionId = connectionId;
        _logger = loggerFactory.CreateLogger<Transaction>();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _transaction.CommitAsync(cancellationToken);
        _logger.LogInformation("Транзакция сommit id соединения {ConnectionId}", _connectionId);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await _transaction.RollbackAsync(cancellationToken);
        _logger.LogInformation("Транзакция сommit id соединения {ConnectionId}", _connectionId);
    }


    public async ValueTask DisposeAsync() => await _transaction.DisposeAsync();
}